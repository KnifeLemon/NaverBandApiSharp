using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaverBandApiSharp.Enums
{
    public enum BandAccountCheckResult
    {
        CAN_CREATE,
        CANT_CREATE,
        ACCOUNT_EXISTS,
        ERROR_SIGNUP_LIMIT_EXCEEDED,
        ERROR_SAME_EMAIL_REGISTERED
    }
}
