using NaverBandApiSharp.API;
using Newtonsoft.Json;

namespace NaverBandApiSharp.Classes.Models
{
    public class MobileDevice
    {
        [JsonProperty("os_version")]
        public string os_version { get; set; } = BandApiConstants.ANDROID_OS_VERSION;

        [JsonProperty("api_level")]
        public string api_level { get; set; } = BandApiConstants.ANDROID_API_LEVEL;

        [JsonProperty("brand")]
        public string brand { get; set; } = BandApiConstants.ANDROID_BRAND;

        [JsonProperty("model")]
        public string model { get; set; } = BandApiConstants.ANDROID_MODEL;

        [JsonProperty("chrome_version")]
        public string chrome_version { get; set; } = BandApiConstants.ANDROID_CHROME_VERSION;
    }
}
