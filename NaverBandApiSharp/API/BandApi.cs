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

        public bool login(BandApiAccountType accountType, string identifiy, string password)
        {
            Login login = new Login(accountType, identifiy, password);

            return true;
        }

    }
}
