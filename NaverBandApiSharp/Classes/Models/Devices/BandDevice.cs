using NaverBandApiSharp.API;
using Newtonsoft.Json;

namespace NaverBandApiSharp.Classes.Models
{
    public class BandDevice
    {
        [JsonProperty("os_name")]
        public string os_name { get; set; } = "android";

        [JsonProperty("os_version")]
        public string os_version { get; set; }

        [JsonProperty("api_level")]
        public string api_level { get; set; }

        [JsonProperty("device")]
        public string device { get; set; }

        [JsonProperty("brand")]
        public string brand { get; set; }

        [JsonProperty("model")]
        public string model { get; set; }

        [JsonProperty("device_id")]
        public string device_id { get; set; }

        [JsonProperty("ad_id")]
        public string ad_id { get; set; } = Guid.NewGuid().ToString();



        [JsonProperty("identify")]
        public string identify { get; set; }

        [JsonProperty("user_no")]
        public int user_no { get; set; }



        [JsonProperty("authorization_type")]
        public string authorization_type { get; set; }

        [JsonProperty("access_token")]
        public string access_token { get; set; }

        [JsonProperty("guest_access_token")]
        public string guest_access_token { get; set; }

        [JsonProperty("di")]
        public string di { get; set; }

        [JsonProperty("enc_hmac_key")]
        public string enc_hmac_key { get; set; }

        [JsonProperty("web_user_agent")]
        public string web_user_agent { get; set; }

        [JsonProperty("band_user_agent")]
        public string band_user_agent { get; set; }
    }
}
