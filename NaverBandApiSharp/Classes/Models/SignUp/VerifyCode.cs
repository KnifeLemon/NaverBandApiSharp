using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NaverBandApiSharp.Classes.Models
{
    public class VerifyCodeOwner
    {

        [JsonProperty("name_hint")]
        public string name_hint { get; set; }
    }

    public class VerifyCodeResult
    {

        [JsonProperty("verification_token")]
        public string verification_token { get; set; }

        [JsonProperty("owner")]
        public VerifyCodeOwner owner { get; set; }

        [JsonProperty("expires_in")]
        public int expires_in { get; set; }
    }
}
