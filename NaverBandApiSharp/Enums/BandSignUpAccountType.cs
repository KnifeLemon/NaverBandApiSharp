using NaverBandApiSharp.Helper;

namespace NaverBandApiSharp.Enums
{
    public enum BandSignUpAccountType
    {
        [StringValue("phone_number")]
        PHONE,
        [StringValue("email")]
        EMAIL,
    }
}