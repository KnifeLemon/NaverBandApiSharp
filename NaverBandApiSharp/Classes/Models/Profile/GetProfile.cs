using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NaverBandApiSharp.Classes.Models
{
    public class PersonalInfoAgreements
    {

        [JsonProperty("birthday")]
        public bool birthday { get; set; }

        [JsonProperty("profile_image")]
        public bool profile_image { get; set; }

        [JsonProperty("shared_location")]
        public bool shared_location { get; set; }

        [JsonProperty("gender")]
        public bool gender { get; set; }

        [JsonProperty("phone_number")]
        public bool phone_number { get; set; }

        [JsonProperty("location")]
        public bool location { get; set; }

        [JsonProperty("event_info_push")]
        public bool event_info_push { get; set; }

        [JsonProperty("event_info_email")]
        public bool event_info_email { get; set; }

        [JsonProperty("npay_remittance")]
        public bool npay_remittance { get; set; }

        [JsonProperty("email")]
        public bool email { get; set; }

        [JsonProperty("contacts")]
        public bool contacts { get; set; }
    }

    public class GuardianshipRestrictions
    {

        [JsonProperty("band_search")]
        public bool band_search { get; set; }
    }

    public class MyInfo
    {

        [JsonProperty("updated_at")]
        public int updated_at { get; set; }

        [JsonProperty("confirmed_at")]
        public long confirmed_at { get; set; }
    }

    public class GetProfileResult
    {

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("cellphone")]
        public string cellphone { get; set; }

        [JsonProperty("face")]
        public string face { get; set; }

        [JsonProperty("thumbnail")]
        public string thumbnail { get; set; }

        [JsonProperty("birthday")]
        public string birthday { get; set; }

        [JsonProperty("naver_id")]
        public string naver_id { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("user_no")]
        public int user_no { get; set; }

        [JsonProperty("need_cellphone_change")]
        public bool need_cellphone_change { get; set; }

        [JsonProperty("need_cellphone_authorize")]
        public bool need_cellphone_authorize { get; set; }

        [JsonProperty("is_lunar")]
        public bool is_lunar { get; set; }

        [JsonProperty("facebook_user_id")]
        public string facebook_user_id { get; set; }

        [JsonProperty("line_user_id")]
        public string line_user_id { get; set; }

        [JsonProperty("google_user_id")]
        public string google_user_id { get; set; }

        [JsonProperty("apple_user_id")]
        public string apple_user_id { get; set; }

        [JsonProperty("whale_user_id")]
        public string whale_user_id { get; set; }

        [JsonProperty("is_exist_password")]
        public bool is_exist_password { get; set; }

        [JsonProperty("is_open_me2day")]
        public bool is_open_me2day { get; set; }

        [JsonProperty("contract_language")]
        public string contract_language { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("country_code")]
        public int country_code { get; set; }

        [JsonProperty("country")]
        public string country { get; set; }

        [JsonProperty("is_email_verified")]
        public bool is_email_verified { get; set; }

        [JsonProperty("birthdate")]
        public string birthdate { get; set; }

        [JsonProperty("age")]
        public int age { get; set; }

        [JsonProperty("is_kid")]
        public bool is_kid { get; set; }

        [JsonProperty("age_group")]
        public string age_group { get; set; }

        [JsonProperty("personal_info_agreements")]
        public PersonalInfoAgreements personal_info_agreements { get; set; }

        [JsonProperty("guardianship_restrictions")]
        public GuardianshipRestrictions guardianship_restrictions { get; set; }

        [JsonProperty("my_info")]
        public MyInfo my_info { get; set; }

        [JsonProperty("user_access_restriction_statuses")]
        public IList<object> user_access_restriction_statuses { get; set; }
    }
}
