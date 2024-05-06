using NaverBandApiSharp.Classes.Models;
using NaverBandApiSharp.Enums;

namespace NaverBandApiSharp.API
{
    /**
     * <summary>
     *      기초 API 인터페이스
     * </summary>
     */
    public interface IBandApi
    {
        void setBandDevice(BandDevice _device);
        BandDevice getBandDevice();

        void setAndroidDevice(MobileDevice _device);
        MobileDevice getAndroidDevice();


        bool SignIn(BandAccountType accountType, string identifiy, string password);



        Task<bool> SignUpStart(BandSignUpAccountType accountType, string identifiy, string password);

        Task<bool> SignUpSendCode();

        Task<bool> SignUpVerifyCode(string code);

        Task<bool> SignUpEnd(string name, DateTime birthdate, bool birthDateSolar = false, bool serviceNotification = false);

        //GetStartToken BandGetStartToken { get; }
    }
}
