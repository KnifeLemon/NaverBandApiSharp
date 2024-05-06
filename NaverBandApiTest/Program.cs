using NaverBandApiSharp.API;
using NaverBandApiSharp.Enums;

BandApi band = new BandApi();
//bandApi.login(NaverBandApiSharp.Enums.BandAccountType.EMAIL, "test", "1234");



//SignUp(BandSignUpAccountType.EMAIL, "example@naver.com", "Abcd1234!", "홍길동", new DateTime(1991, 01, 23));
//SignUp(BandSignUpAccountType.PHONE, "+821012345678", "Abcd1234!", "홍길동", new DateTime(1991, 01, 23));

async void SignUp(BandSignUpAccountType accountType, string identify, string password, string name, DateTime birthDate, bool isSolarCalendar = false, bool allowServiceNoification = false)
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


Console.ReadLine();