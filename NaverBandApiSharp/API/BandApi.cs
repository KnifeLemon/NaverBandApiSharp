using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaverBandApiSharp.Classes.Models;
using NaverBandApiSharp.Enums;
using NaverBandApiSharp.Helper;

namespace NaverBandApiSharp.API
{

    /**
     * <summary>
     *      네이버 밴드 API
     * </summary>
     */
    public class BandApi : IBandApi
    {
        private MobileDevice mobile_device;
        private BandDevice band_device;

        /**
         * <summary>
         *      네이버 밴드 API 생성자
         * </summary>
         */
        public BandApi()
        {
        }

        public void setBandDevice(BandDevice _device)
        {
            this.band_device = _device;
        }

        public BandDevice getBandDevice()
        {
            return band_device;
        }

        public void setAndroidDevice(MobileDevice _device)
        {
            mobile_device = _device;
        }

        public MobileDevice getAndroidDevice()
        {
            return mobile_device;
        }

        #region SignIn
        private SignIn signIn;
        public bool SignIn(BandAccountType accountType, string identifiy, string password)
        {
            signIn = new SignIn(accountType, identifiy, password);

            return true;
        }

        #endregion

        #region Sign-Up
        private SignUp signUp;
        public async Task<bool> SignUpStart(BandSignUpAccountType signUpAccountType, string identifiy, string password)
        {
            signUp = new SignUp();

            return await signUp.Ready(signUpAccountType, identifiy, password);
        }

        public async Task<bool> SignUpSendCode()
        {
            return await signUp.SendAuth();
        }

        public async Task<bool> SignUpVerifyCode(string code)
        {
            return await signUp.VerifyCode(code);
        }

        public async Task<bool> SignUpEnd(string name, DateTime birthdate, bool birthDateSolar = false, bool serviceNotification = false)
        {
            return await signUp.Finish(name, birthdate, birthDateSolar, serviceNotification);
        }

        #endregion



        //public bool SignUp(BandSignUpAccountType signUpAccountType, )

    }
}
