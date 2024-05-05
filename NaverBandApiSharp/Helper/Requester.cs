using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using NaverBandApiSharp.API;
using NaverBandApiSharp.Classes.Models;
using Newtonsoft.Json;

namespace NaverBandApiSharp.Helper
{
    internal class Requester
    {
        private CookieContainer cookieContainer = new CookieContainer();

        internal Requester()
        {
        }

        private HttpRequestMessage GetDefaultRequest(HttpMethod method, string url)
        {
            var request = new HttpRequestMessage(method, url);
            request.Headers.Add(BandApiConstants.HEADER_HOST, new Uri(url).Host);
            request.Headers.Add(BandApiConstants.HEADER_DEVICE_TIME_ZONE_ID, BandApiConstants.TIMEZONE);
            request.Headers.Add(BandApiConstants.HEADER_COUNTRY, BandApiConstants.COUNTRY);
            request.Headers.Add(BandApiConstants.HEADER_AKEY, BandApiConstants.APP_KEY);
            request.Headers.Add(BandApiConstants.HEADER_DEVICE_TIME_ZONE_MS_OFFSET, BandApiConstants.TIMEZONE_OFFSET.ToString());
            request.Headers.Add(BandApiConstants.HEADER_LANGUAGE, BandApiConstants.LANGUAGE);
            request.Headers.Add(BandApiConstants.HEADER_USER_AGENT, BandApiConstants.USER_AGENT_DEFAULT);
            request.Headers.Add(BandApiConstants.HEADER_VERSION, BandApiConstants.VERSION);
            request.Headers.Add(BandApiConstants.HEADER_ACCEPT_ENCODING, BandApiConstants.ACCEPT_ENCODING);
            return request;
        }

        public async Task<T?> send<T>(HttpMethod method, string url)
        {
            var request = GetDefaultRequest(method, url);

            return await response<T>(request);
        }

        public async Task<T?> send<T>(HttpMethod method, string url, Dictionary<string, string> data)
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

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new RequestFailedException("response status code is not OK")
                    {
                        code = (int)response.StatusCode,
                        response = json,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(json);
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
