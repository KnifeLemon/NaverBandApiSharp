using Newtonsoft.Json;

namespace NaverBandApiSharp.Classes.Models
{
    public class GetEmailStatusResult
    {

        [JsonProperty("email_status")]
        public string email_status { get; set; }
    }
}
