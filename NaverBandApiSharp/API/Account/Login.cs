using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NaverBandApiSharp.Classes.Models;
using NaverBandApiSharp.Enums;
using NaverBandApiSharp.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace NaverBandApiSharp.API
{
    internal sealed class Login : BandApi
    {
        private BandApiAccountType accountType;
        private string id;
        private string password;

        public Login(BandApiAccountType _accountType, string _id, string _password)
        {
            id = _id;
            password = _password;

            Credential credential = new Credential();
            credential.StartBand();
        }

        
    }
}
