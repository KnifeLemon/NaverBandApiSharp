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
    internal class BandApi : IBandApi
    {
        private BandApiAccountType accountType;
        private string id;
        private string password;


        /**
         * <summary>
         *      네이버 밴드 API 생성자
         * </summary>
         */
        internal BandApi(BandApiAccountType _accountType, string _id, string _password)
        {
            accountType = _accountType;
            id = _id;
            password = _password;

            /**
             * 네이버 밴드 앱 인증 {
             */

            Task.Run(async () =>
            {
                try
                {
                    // 인증 요청
                    Requester req = new Requester();
                    var startBand = req.send<GetStartToken>(HttpMethod.Get, BandApiConstants.STARTUP_GET_START_TOKEN);


                }
                catch (RequestFailedException ex)
                {
                    // 인증 요청 실패
                    throw new Exception(ex.Message, ex);
                }
                catch (Exception ex)
                {
                    // 인증 요청 실패
                    throw new Exception(ex.Message, ex);
                }
            }).Wait();



            /**
             * } 네이버 밴드 앱 인증
             */
        }
    }
}
