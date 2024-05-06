using NaverBandApiSharp.API;
using NaverBandApiSharp.Enums;

namespace NaverBandApiTest.Account
{
    internal class SignUpExample
    {
        BandApi band = new BandApi();

        // 이메일 계정 생성
        //SignUp(BandSignUpAccountType.EMAIL, "example@naver.com", "Abcd1234!", "홍길동", new DateTime(1991, 01, 23)).Wait();

        // 휴대폰 계정 생성 (아이디는 국가번호 포함 되어야함)
        //SignUp(BandSignUpAccountType.PHONE, "+821012345678", "Abcd1234!", "홍길동", new DateTime(1991, 01, 23)).Wait();


        async Task SignUp(BandSignUpAccountType accountType, string identify, string password, string name, DateTime birthDate, bool isSolarCalendar = false, bool allowServiceNoification = false)
        {
            try
            {
                bool start = await band.SignUpStart(accountType, identify, password);
                if (start == false)
                {
                    Console.WriteLine("가입 요청 실패");
                    return;
                }
                goto sendCode;

            sendCode:
                bool sendCodeResult = await band.SignUpSendCode();
                if (sendCodeResult == false)
                {
                    Console.WriteLine("코드 발송 실패");
                    return;
                }
                Console.WriteLine("인증번호가 발송됐습니다.");
                Console.WriteLine("");

            tryVerify:
                Console.WriteLine("재발송 하려면 re를 입력하세요.");
                Console.Write("인증번호를 입력하세요 : ");
                string code = Console.ReadLine();
                if (string.IsNullOrEmpty(code))
                    goto tryVerify;
                else if (code.ToLower() == "re")
                    goto sendCode;

                bool verifyResult = await band.SignUpVerifyCode(code);
                if (verifyResult == false)
                {
                    Console.WriteLine("인증번호를 다시 입력해주세요.");
                    goto tryVerify;
                }
                goto successVerify;

            successVerify:
                Console.WriteLine("인증에 성공했습니다.");

                bool complete = await band.SignUpEnd(name, birthDate, isSolarCalendar, allowServiceNoification);
                if (complete == false)
                {
                    Console.WriteLine("회원가입에 실패했습니다.");
                    return;
                }

                Console.WriteLine("회원가입 성공");
            }
            catch (SignUpFailedException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }
    }
}
