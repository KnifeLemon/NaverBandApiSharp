using Newtonsoft.Json;

namespace NaverBandApiSharp.Classes.Models
{
    public class GetStartTokenResult
    {
        [JsonProperty("token")]
        public string token { get; set; }
    }
}
