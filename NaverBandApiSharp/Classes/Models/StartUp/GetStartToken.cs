using Newtonsoft.Json;

namespace NaverBandApiSharp.Classes.Models
{
    public class GetStartTokenResultData
    {
        [JsonProperty("message")]
        public string? message { get; set; }

        [JsonProperty("token")]
        public string token { get; set; }
    }

    public class GetStartToken
    {

        [JsonProperty("result_code")]
        public int result_code { get; set; }

        [JsonProperty("result_data")]
        public GetStartTokenResultData result_data { get; set; }
    }
}
