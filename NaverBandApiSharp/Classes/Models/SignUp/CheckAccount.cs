using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NaverBandApiSharp.Classes.Models
{
    public class CanNotCreateReason
    {
        [JsonProperty("code")]
        public string code { get; set; }

        [JsonProperty("message")]
        public string message { get; set; }
    }

    public class CheckAccountResult
    {

        [JsonProperty("can_create")]
        public bool can_create { get; set; }

        [JsonProperty("account_exists")]
        public bool account_exists { get; set; }

        [JsonProperty("can_not_create_reasons")]
        public IList<CanNotCreateReason> can_not_create_reasons { get; set; }
    }

}
