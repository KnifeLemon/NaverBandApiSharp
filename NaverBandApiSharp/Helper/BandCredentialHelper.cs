using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace NaverBandApiSharp.Helper
{
    internal static class BandCredentialHelper
    {
        public static string GetHashedMdString(this string data, string key)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var derivedKey = Convert.FromBase64String(key);
            var digest = new HMac(new Org.BouncyCastle.Crypto.Digests.Sha256Digest());

            digest.Init(new KeyParameter(derivedKey));
            digest.BlockUpdate(dataBytes, 0, dataBytes.Length);

            var output = new byte[digest.GetMacSize()];
            digest.DoFinal(output, 0);
            digest.Reset();

            return Convert.ToBase64String(output);
        }

        public static string EncryptRSA(string strPublicModulusKey, string strPublicExponentKey, string strTarget)
        {
            string strResult = String.Empty;
            RSAParameters publicKey = new RSAParameters()
            {
                Modulus = Enumerable.Range(0, strPublicModulusKey.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(strPublicModulusKey.Substring(x, 2), 16))
                .ToArray()
                ,
                Exponent = Enumerable.Range(0, strPublicExponentKey.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(strPublicExponentKey.Substring(x, 2), 16))
                .ToArray()
            };

            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(publicKey);
                byte[] enc = rsa.Encrypt(Encoding.UTF8.GetBytes(strTarget), false);
                strResult = encode(enc);
            }
            catch (CryptographicException ex)
            {
                strResult = String.Empty;
            }
            return strResult;
        }

        private static readonly char[] enc_charAry = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/' };
        private static readonly int[] enc_intAry = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 62, 0, 0, 0, 63, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 0, 0, 0, 0, 0, 0, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51 };
        private static bool enc_CRLF = false;
        private static readonly int ACTION_MASK = 255;
        private static readonly int ACTION_POINTER_INDEX_MASK = 65280;
        private static string encode(byte[] bArr)
        {
            StringBuilder stringBuffer = new StringBuilder(bArr.Length);
            MemoryStream byteArrayInputStream = new MemoryStream(bArr);
            int i = 0;
            for (int i2 = 0; i2 < bArr.Count(); i2 += 3)
            {
                int read = byteArrayInputStream.ReadByte();
                int read2 = byteArrayInputStream.ReadByte();
                int read3 = byteArrayInputStream.ReadByte();
                if (read2 == -1)
                {
                    i = 2;
                    read2 = 0;
                }
                else if (read3 == -1)
                {
                    i = 1;
                    read3 = 0;
                }
                int i3 = (read3 << 0) & ACTION_MASK;
                int i4 = i3 | ((read2 << 8) & ACTION_POINTER_INDEX_MASK) | ((read << 16) & 16711680);
                stringBuffer.Append(enc_charAry[(i4 >> 18) & 63]);
                stringBuffer.Append(enc_charAry[(i4 >> 12) & 63]);
                stringBuffer.Append(enc_charAry[(i4 >> 6) & 63]);
                stringBuffer.Append(enc_charAry[(i4 >> 0) & 63]);
                if ((i2 + 3) % 57 == 0 && enc_CRLF)
                {
                    stringBuffer.Append(10);
                }
            }
            int length = stringBuffer.Length;
            while (i > 0)
            {
                stringBuffer[length - i] = '=';
                i--;
            }
            if (enc_CRLF)
            {
                stringBuffer.Append(10);
            }
            return stringBuffer.ToString();
        }
    }
}
