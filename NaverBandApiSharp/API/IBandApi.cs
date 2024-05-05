using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        bool login(BandApiAccountType accountType, string identifiy, string password);

        //GetStartToken BandGetStartToken { get; }
    }
}
