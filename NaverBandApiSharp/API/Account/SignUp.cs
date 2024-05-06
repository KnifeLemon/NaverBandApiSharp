using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using NaverBandApiSharp.Classes.Lists;
using NaverBandApiSharp.Classes.Models;
using NaverBandApiSharp.Enums;
using NaverBandApiSharp.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Ocsp;

namespace NaverBandApiSharp.API
{
    internal sealed class SignUp : BandApi
    {
        private BandDevice device;
        private Requester req;

        private BandSignUpAccountType accountType;
        private string identify;
        private string password;
        private int verificationId;
        private string verificationToken;

        public SignUp()
        {
            //device = this.getBandDevice();
            //req = new Requester(device, true);
        }

        /**
        * <summary>
        *      [Step1] 가입을 진행합니다. true를 반환할경우 인증 준비 완료 상태입니다.
        * </summary>
        */
        public async Task<bool> Ready(BandSignUpAccountType _accountType, string _identify, string _password)
        {
            this.accountType = _accountType;
            this.identify = _identify;
            this.password = _password;

            // 인증 초기화
            Credential credential = new Credential();
            device = await credential.StartBand();
            req = new Requester(device, true);

            // 계정 등록
            device.identify = _identify;

            // 계정 체크
            BandAccountCheckResult accountCheckResult = await CheckAccount(accountType, identify);
            if (accountCheckResult == BandAccountCheckResult.CANT_CREATE)
                throw new SignUpFailedException("계정을 생성할 수 없습니다.");
            else if (accountCheckResult == BandAccountCheckResult.ERROR_SIGNUP_LIMIT_EXCEEDED)
                throw new SignUpFailedException("동일한 계정의 반복적 사용으로 추가 등록이 제한되었습니다. 다른 계정정보를 등록해 주세요.");
            else if (accountCheckResult == BandAccountCheckResult.ERROR_SAME_EMAIL_REGISTERED)
                throw new SignUpFailedException("사용 중인 이메일입니다. 당신의 이메일주소가 맞다면, 가입한 정보로 로그인해주세요.");

            // 비밀번호 체크
            bool passwordCheckResult = await CheckPassword(password);
            if (passwordCheckResult == false)
                throw new SignUpFailedException("비밀번호를 사용할 수 없습니다. 대문자, 소문자, 숫자, 특수문자를 섞어주세요.");

            if (accountType == BandSignUpAccountType.EMAIL)
            {
                var resEmailStatus = await req.send<GetEmailStatusResult>(HttpMethod.Get, string.Format(BandApiConstants.SIGNUP_GET_EMAIL_STATUS, "false", identify));
                if (resEmailStatus.email_status == "used")
                    throw new SignUpFailedException("이미 가입된 이메일 입니다.");
            }

            return true;
        }

        /**
         * <summary>
         *      [Step2] 휴대폰으로 인증번호를 발송합니다.
         * </summary>
         */
        public async Task<bool> SendAuth()
        {
            try
            {
                if (accountType == BandSignUpAccountType.EMAIL)
                {
                    var resSendCode = await req.send<SendAuthEmailResult>(HttpMethod.Post, BandApiConstants.SIGNUP_SEND_AUTH_EMAIL, new Dictionary<string, string>()
                    {
                        { "email", identify }
                    });

                    verificationId = resSendCode.verification_code_id;
                }
                else if (accountType == BandSignUpAccountType.PHONE)
                {
                    await req.send<MessageCommonResult>(HttpMethod.Post, BandApiConstants.SIGNUP_SEND_AUTH_SMS, new Dictionary<string, string>()
                    {
                        { "phone_number", identify }
                    });
                }

                // 오류없이 넘어왔다면 성공임
                // {"result_code":1,"result_data":{"message":"처리되었습니다."}}
                return true;
            }
            catch (RequestFailedException ex)
            {
                return false;
            }
        }

        /**
         * <summary>
         *      [Step3] 발송된 인증번호를 검증 요청합니다.
         * </summary>
         */
        public async Task<bool> VerifyCode(string code)
        {
            try
            {
                string accountTypeString = accountType.GetStringValue();

                string verifyURL = "";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

                if (accountType == BandSignUpAccountType.EMAIL)
                {
                    verifyURL = BandApiConstants.SIGNUP_VERIFY_EMAIL;
                    keyValuePairs = new Dictionary<string, string>()
                    {
                        { "verification_code_id", verificationId.ToString() },
                        { "email", identify },
                        { "verification_code", code }
                    };
                }
                else if (accountType == BandSignUpAccountType.PHONE)
                {
                    verifyURL = BandApiConstants.SIGNUP_VERIFY_CODE;
                    keyValuePairs = new Dictionary<string, string>()
                    {
                        { "type", accountTypeString },
                        { "type_id", identify },
                        { "verification_code", code }
                    };
                }

                var resVerifyCode = await req.send<VerifyCodeResult>(HttpMethod.Post, verifyURL, keyValuePairs);
                // {"result_code":1,"result_data":{"verification_token":"aNm4M6p4CJBuqIT0l+HTlBI4jlRcA+BTufFh1SmXheI=","owner":{"name_hint":"홍길*"},"expires_in":3599}}
                verificationToken = resVerifyCode.verification_token;
                return true;
            }
            catch (RequestFailedException ex)
            {
                // {"result_code":1007,"result_data":{"message":"인증 번호가 올바르지 않습니다."}}
                return false;
            }
        }

        /**
         * <summary>
         *      [Step4] 회원가입 요청을 보냅니다.
         * </summary>
         */
        public async Task<bool> Finish(string name, DateTime birthdate, bool birthDateSolar = false, bool serviceNotification = false)
        {
            string accountTypeString = accountType.GetStringValue();

            JObject payload = PayloadLoader.Get("SignUpPayload.json");

            payload["client"]["country"] = BandApiConstants.COUNTRY;
            payload["client"]["client_context"]["adid"] = device.ad_id;
            payload["client"]["os_ver"] = device.os_version;
            payload["client"]["device_id"] = device.device_id;
            payload["client"]["device_model"] = device.device;
            payload["client"]["app_ver"] = BandApiConstants.APP_VERSION;
            payload["client"]["os_name"] = device.os_name;
            payload["client"]["language"] = BandApiConstants.LANGUAGE.Substring(0, 2).ToLower();

            payload["events"][0]["event_time"] = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
            payload["events"][0]["extra"]["method"] = accountTypeString.Split('_')[0];
            payload["events"][0]["extra"]["user_verify_id"] = device.ad_id;

            //long ts = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            //string signup_payload = "{\"client\": {\"country\":\"" + BandApiConstants.COUNTRY+ "\",\"client_context\":{\"adid\":\"" + device.ad_id + "\"},\"product\":\"bandapp\",\"os_ver\":\"" + device.os_version + "\",\"device_id\":\"" + device.device_id + "\",\"device_model\":\"" + device.device + "\",\"service_id\":\"band\",\"app_ver\":\"" + BandApiConstants.APP_VERSION + "\",\"user_key\":\"__UNKNOWN__\",\"os_name\":\"" + device.os_name + "\",\"language\":\"" + BandApiConstants.LANGUAGE.Substring(0, 2).ToLower() + "\"}, \"events\": [{\"scene_id\":\"app_signup_background\",\"action_id\":\"occur\",\"classifier\":\"server_signup_complete\",\"event_time\":" + ts.ToString() + ",\"extra\":{\"method\":\"" + accountTypeString.Split('_')[0] + "\",\"user_verify_id\":\"" + device.ad_id + "\"}}]}";


            var resSignUp = await req.send<SignUpResult>(HttpMethod.Post, BandApiConstants.SIGNUP, new Dictionary<string, string>()
            {
                { "account_type", accountTypeString },
                { "service_notification", (serviceNotification ? "true" : "false") },
                { "password", password },
                { "sim_operator", PLMN.GetRandomPLMN() },
                { "birthdate", birthdate.ToString("yyyyMMdd") + (birthDateSolar ? "-" : "+") },
                { "device_id", device.device_id },
                { "device_model", device.device },
                { "ba_signup_payload", JsonConvert.SerializeObject(payload) },
                //{ "ba_signup_payload", signup_payload },
                { "name", name },
                { "language", BandApiConstants.LANGUAGE },
                { "time_zone_id", BandApiConstants.TIMEZONE },
                { "verification_token", verificationToken }
            });

            device.user_no = resSignUp.user_no;
            device.authorization_type = resSignUp.type;
            device.access_token = resSignUp.access_token;

            this.setBandDevice(device);

            return true;
        }

        #region Common

        /**
         * <summary>
         *      아이디 생성 가능 유무를 확인합니다.
         * </summary>
         */
        public async Task<BandAccountCheckResult> CheckAccount(BandSignUpAccountType accountType, string identify)
        {
            if (accountType == BandSignUpAccountType.EMAIL && identify.Contains("@") == false)
            {
                throw new SignUpFailedException("이메일 형식이 올바르지 않습니다.");
            }
            if (accountType == BandSignUpAccountType.PHONE && identify.StartsWith("+") == false)
            {
                throw new SignUpFailedException("휴대폰 번호는 +를 포함한 국가번호로 시작되어야합니다.");
            }

            string accountTypeString = accountType.GetStringValue();

            var resCheckAccount = await req.send<CheckAccountResult>(HttpMethod.Get, string.Format(BandApiConstants.SIGNUP_CHECK_ACCOUNT, accountTypeString, identify));

            if (resCheckAccount.can_create && resCheckAccount.account_exists == false)
            {
                return BandAccountCheckResult.CAN_CREATE;
            }
            else if (resCheckAccount.can_create && resCheckAccount.account_exists)
            {
                return BandAccountCheckResult.ACCOUNT_EXISTS;
            }
            else
            {
                if (resCheckAccount.can_not_create_reasons != null)
                {
                    foreach (var item in resCheckAccount.can_not_create_reasons)
                    {
                        if (item.code == "ERROR_SIGNUP_LIMIT_EXCEEDED")
                            return BandAccountCheckResult.ERROR_SIGNUP_LIMIT_EXCEEDED;
                        else if (item.code == "ERROR_SAME_EMAIL_REGISTERED")
                            return BandAccountCheckResult.ERROR_SAME_EMAIL_REGISTERED;
                    }
                }

                return BandAccountCheckResult.CANT_CREATE;
            }
        }

        /**
         * <summary>
         *      가입 가능한 비밀번호인지 체크합니다.
         * </summary>
         */
        public async Task<bool> CheckPassword(string password)
        {
            var resCheckPassword = await req.send<CheckPasswordResult>(HttpMethod.Post, BandApiConstants.SIGNUP_CHECK_PASSWORD, new Dictionary<string, string>()
            {
                { "password", password }
            });

            return (resCheckPassword.grade == "STRONG");
        }

        #endregion
    }
}
