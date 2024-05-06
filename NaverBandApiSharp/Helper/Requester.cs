using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text.Json.Nodes;
using NaverBandApiSharp.API;
using NaverBandApiSharp.Classes.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;

namespace NaverBandApiSharp.Helper
{
    internal class Requester
    {
        private CookieContainer cookieContainer = new CookieContainer();
        private BandDevice device;
        private bool createMD = false;

        internal Requester(BandDevice bandDevice, bool _createMD = false)
        {
            device = bandDevice;
            createMD = _createMD;
        }

        private HttpRequestMessage GetDefaultRequest(HttpMethod method, string url)
        {
            long ts = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            if (url.Contains("?ts="))
                url = url.Replace("?ts=", "?ts=" + ts.ToString());
            if (url.Contains("&ts="))
                url = url.Replace("&ts=", "&ts=" + ts.ToString());

            var request = new HttpRequestMessage(method, url);
            request.Headers.Add(BandApiConstants.HEADER_HOST, new Uri(url).Host);
            request.Headers.Add(BandApiConstants.HEADER_DEVICE_TIME_ZONE_ID, BandApiConstants.TIMEZONE);
            request.Headers.Add(BandApiConstants.HEADER_COUNTRY, BandApiConstants.COUNTRY);
            request.Headers.Add(BandApiConstants.HEADER_AKEY, BandApiConstants.APP_KEY);
            request.Headers.Add(BandApiConstants.HEADER_DEVICE_TIME_ZONE_MS_OFFSET, BandApiConstants.TIMEZONE_OFFSET.ToString());
            request.Headers.Add(BandApiConstants.HEADER_LANGUAGE, BandApiConstants.LANGUAGE);
            request.Headers.TryAddWithoutValidation(BandApiConstants.HEADER_USER_AGENT, (device.band_user_agent != null ? device.band_user_agent : BandApiConstants.USER_AGENT_DEFAULT));
            request.Headers.Add(BandApiConstants.HEADER_VERSION, BandApiConstants.VERSION);
            request.Headers.Add(BandApiConstants.HEADER_ACCEPT_ENCODING, BandApiConstants.ACCEPT_ENCODING);

            if (device.access_token != null)
                request.Headers.Add(BandApiConstants.HEADER_Authorization, $"{device.authorization_type} {device.access_token}");
            else if (device.guest_access_token != null)
                request.Headers.Add(BandApiConstants.HEADER_Authorization, $"Bearer {device.guest_access_token}");

            Console.WriteLine("URL : " + url);
            if (createMD)
                request.Headers.Add(BandApiConstants.HEADER_MD, url.GetUrlPathAndQuery().GetHashedMdString(device.enc_hmac_key));
            return request;
        }

        private HttpRequestMessage GetDefaultRequest(HttpMethod method, string url, Dictionary<string, string> data)
        {
            var request = GetDefaultRequest(method, url);
            foreach (var item in data.ToDictionary(entry => entry.Key, entry => entry.Value))
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    data.Remove(item.Key);
                }
            }
            request.Content = new FormUrlEncodedContent(data);
            return request;
        }

        public async Task<T?> send<T>(HttpMethod method, string url)
        {
            var request = GetDefaultRequest(method, url);

            return await response<T>(request);
        }

        public async Task<T?> send<T>(HttpMethod method, string url, Dictionary<string, string> data)
        {
            var request = GetDefaultRequest(method, url, data);

            return await response<T>(request);
        }

        private async Task<T?> response<T>(HttpRequestMessage request)
        {
            try
            {
                var clientHandler = new HttpClientHandler
                {
                    AllowAutoRedirect = true,
                    UseCookies = true,
                    CookieContainer = cookieContainer,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                };
                
                HttpClient client = new HttpClient(clientHandler);
                var response = await client.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(" = RESPONSE BODY = ");
                Console.WriteLine(json);

                JObject tempJson = JObject.Parse(json);

                if (tempJson["result_code"].ToString() != "1" && tempJson["result_data"]["message"] != null)
                {
                    throw new RequestFailedException(tempJson["result_data"]["message"].ToString())
                    {
                        code = (int)response.StatusCode,
                        response = json,
                    };
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new RequestFailedException("response status code is not OK")
                    {
                        code = (int)response.StatusCode,
                        response = json,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(tempJson["result_data"].ToString());
                return obj;
            }
            catch (HttpRequestException httpException)
            {
                throw new RequestFailedException(httpException.Message)
                {
                    code = (int)httpException.StatusCode,
                    response = httpException.StackTrace,
                };
            }
            catch (Exception exception)
            {
                throw new RequestFailedException(exception.Message)
                {
                    code = (int)exception.HResult,
                    response = exception.StackTrace,
                };
            }
        }

    }
}
