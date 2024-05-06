using Newtonsoft.Json;

namespace NaverBandApiSharp.Classes.Models
{
    public class MessageCommonResult
    {

        [JsonProperty("message")]
        public string message { get; set; }
    }
}
