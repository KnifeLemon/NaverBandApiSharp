using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaverBandApiSharp.Classes.Models;
using NaverBandApiSharp.Helper;

namespace NaverBandApiSharp.API
{
    internal sealed class Profile : BandApi
    {
        BandDevice device;
        Requester req;

        public Profile()
        {
            device = this.getBandDevice();
            req = new Requester(device, true);
        }

        public async Task<GetProfileResult> GetProfile()
        {
            return await req.send<GetProfileResult>(HttpMethod.Get, BandApiConstants.PROFILE_GET);
        }

        public async Task<bool> SetDeviceLanguage()
        {
            try
            {
                var resGetProfile = await req.send<GetProfileResult>(HttpMethod.Post, string.Format(BandApiConstants.STARTUP_SET_DEVICE_LANGUAGE, device.device_id, BandApiConstants.LANGUAGE));
                // 오류 없다면 성공
                // { "result_code":1,"result_data":{ "message":"처리되었습니다."} }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> RenewOnlineStatus()
        {
            try
            {
                var resRenew = await req.send<GetProfileResult>(HttpMethod.Post, BandApiConstants.STARTUP_RENEW_ONLINE_STATUS);
                // 오류 없다면 성공
                // { "result_code":1,"result_data":{ "message":"처리되었습니다."} }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> CheckPhoneRegistrationRequired()
        {
            var resCheck = await req.send<CheckPhoneRegistrationRequiredResult>(HttpMethod.Post, BandApiConstants.PROFILE_CHECK_PHONE_REGISTRATION_REQUIRED);
            return resCheck.phone_registration_required;
        }
    }
}
