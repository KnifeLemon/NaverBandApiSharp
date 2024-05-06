
using Newtonsoft.Json;

namespace NaverBandApiSharp.Classes.Models
{
    public class SendAuthEmailResult
    {

        [JsonProperty("verification_code_id")]
        public int verification_code_id { get; set; }
    }

}
