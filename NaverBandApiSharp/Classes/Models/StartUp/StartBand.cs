﻿using Newtonsoft.Json;

namespace NaverBandApiSharp.Classes.Models
{
    public class StartBandResult
    {
        [JsonProperty("message")]
        public string? message { get; set; }

        [JsonProperty("access_token")]
        public string access_token { get; set; }

        [JsonProperty("expires_in")]
        public int expires_in { get; set; }

        [JsonProperty("enc_hmac_key")]
        public string enc_hmac_key { get; set; }

        [JsonProperty("timestamp")]
        public long timestamp { get; set; }

        [JsonProperty("di")]
        public string di { get; set; }

        [JsonProperty("device_no")]
        public int device_no { get; set; }
    }
}
