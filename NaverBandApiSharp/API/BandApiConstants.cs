using System;

namespace NaverBandApiSharp.API
{
    internal static class BandApiConstants
    {
        #region Main
        public const string RSA_EXPONENT = "010001";
        public const string RSA_MODULUS = "CDDE7FBC056BE809FA53210B27F48545068A32E6E93F07AB7959842842C49999B025FCD883450831A4E6C7BBB2890CB6C1C573D151074B7D6BEB9DB559171B52D0865D0E16CE30E9ADEA9940564405FD2FB9F409FEAC0D88F7BEB3F98224603D422EA965E532C00BFC0E1B1EC89ECA16A840919EA7A84F2922C6ECCFC5AD09FE077A37E0C495A693ED4869B24DF114890D3990539D88EF845B3CBD9C80E12D0CE752B5DC7F2708F4D13EE4754C9BA961D3E0A99CC83B9ACCB4C852F085E6EB48F9ACF102B6DC40E6C230F6267E6A77BCFDF0D10CC49ACD6970ECEBC83117D496D0E0DDCB87EC0E9F8A5E9D2D8E5C9D7A6B2C0A45523ABF0646670615534DD0F3";

        public const string APP_VERSION = "16.0.3";
        public const string APP_KEY = "ea55718a56a32baa6846234bf3bbf9c0";
        public const string APP_SECRET_KEY = "9c6cbbc25ba16be690840cfd4fbaed66";

        public const string USER_AGENT = "Band/{1}({2};{3})"; // Band/{appVersion}({androidOS};{deviceName})
        public const string USER_AGENT_DEFAULT = "Band/16.0.3(Android OS 9;samsung SM-G973N)";

        public const string COUNTRY = "KR";
        public const string LANGUAGE = "ko_KR";

        public const string TIMEZONE = "Asia/Seoul";
        public const int TIMEZONE_OFFSET = 32400000;

        public const string VERSION = "20140411";

        public const string ACCEPT_ENCODING = "gzip, deflate, br";

        public const string HEADER_DEVICE_TIME_ZONE_ID = "Device-Time-Zone-Id";
        public const string HEADER_DEVICE_TIME_ZONE_MS_OFFSET = "Device-Time-Zone-Ms-Offset";
        public const string HEADER_AKEY = "Akey";
        public const string HEADER_COUNTRY = "Country";
        public const string HEADER_LANGUAGE = "Language";
        public const string HEADER_VERSION = "Version";
        public const string HEADER_USER_AGENT = "User-Agent";
        public const string HEADER_ACCEPT_ENCODING = "Accept-Encoding";
        public const string HEADER_HOST = "Host";

        public const string API_URI = "api.band.us";
        public const string AUTH_URI = "auth.band.us";
        public const string PASS_URI = "pass.band.us";
        public const string BAPI_URI = "bapi.band.us";
        public const string SOS_URI = "kr-sos.band.us";
        public const string BAND_API_URL = $"https://{API_URI}";
        public const string BAND_AUTH_URL = $"https://{AUTH_URI}";
        public const string BAND_PASS_URL = $"https://{PASS_URI}";
        public const string BAND_BAPI_URL = $"https://{BAPI_URI}";
        public const string BAND_SOS_URL = $"https://{SOS_URI}";

        #endregion

        #region StartUp Endpoints
        public const string STARTUP_GET_START_TOKEN = BAND_API_URL + "/v1/get_start_token";
        public const string STARTUP_START_BAND = BAND_API_URL + "/v2.0.0/start_band";
        #endregion
    }
}
