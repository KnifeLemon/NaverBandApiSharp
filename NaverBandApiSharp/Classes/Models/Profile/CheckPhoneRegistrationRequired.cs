using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NaverBandApiSharp.Classes.Models
{
    public class CheckPhoneRegistrationRequiredResult
    {

        [JsonProperty("phone_registration_required")]
        public bool phone_registration_required { get; set; }
    }
}
