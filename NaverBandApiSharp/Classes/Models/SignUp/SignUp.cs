using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NaverBandApiSharp.Classes.Models
{
    public class SignUpResult
    {

        [JsonProperty("user_no")]
        public int user_no { get; set; }

        [JsonProperty("access_token")]
        public string access_token { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("is_service_notification")]
        public bool is_service_notification { get; set; }
    }
}
