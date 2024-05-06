using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    internal sealed class SignIn : BandApi
    {
        private BandAccountType accountType;
        private string id;
        private string password;

        private BandDevice device;

        public SignIn(BandAccountType _accountType, string _id, string _password)
        {
            id = _id;
            password = _password;
        }

        public async void Initialize()
        {
            Credential credential = new Credential();
            device = await credential.StartBand();
        }

        
    }
}
