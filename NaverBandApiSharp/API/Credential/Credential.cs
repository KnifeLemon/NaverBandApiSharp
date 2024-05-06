using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaverBandApiSharp.Classes.Models;
using NaverBandApiSharp.Helper;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace NaverBandApiSharp.API
{
    internal sealed class Credential : BandApi
    {
        BandDevice bandDevice;

        public Credential()
        {
            MobileDevice device = this.getAndroidDevice();
            if (device == null)
                device = new MobileDevice();

            bandDevice = BandDeviceHelper.MakeAndroidDevice(device);
        }

        /**
         * <summary>
         *      앱 시작 인증부
         * </summary>
         */
        public async Task<BandDevice> StartBand()
        {
            try
            {
                Requester req = new Requester(bandDevice);

                string privateKey = "";
                string publicKey = "";
                string credential = "";

                // 시작 토큰 요청
                var resStartToken = await req.send<GetStartTokenResult>(HttpMethod.Get, BandApiConstants.STARTUP_GET_START_TOKEN);
                string startToken = resStartToken.token;

                //token key RSA암호화
                string RSACredential = BandCredentialHelper.EncryptRSA(BandApiConstants.RSA_MODULUS, BandApiConstants.RSA_EXPONENT, startToken);

                //RSA KeyPair을 만들어서 RSA Key 를 생성함
                RsaKeyPairGenerator keyPairGenerator = new RsaKeyPairGenerator();
                keyPairGenerator.Init(new Org.BouncyCastle.Crypto.KeyGenerationParameters(new SecureRandom(), 2048));
                AsymmetricCipherKeyPair newKeyPair = keyPairGenerator.GenerateKeyPair();

                //Private Key (PKCS8)
                PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(newKeyPair.Private);
                byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();
                privateKey = Convert.ToBase64String(serializedPrivateBytes);

                //Public Key (PKCS1)
                SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(newKeyPair.Public);
                byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
                publicKey = Convert.ToBase64String(serializedPublicBytes);

                // 인증에 필요없는 요소 제거함 (전송준비)
                publicKey = publicKey.Replace("-----BEGIN PUBLIC KEY-----" + Environment.NewLine, "").Replace("-----END PUBLIC KEY-----", "");
                privateKey = privateKey.Replace("-----BEGIN RSA PRIVATE KEY-----" + Environment.NewLine, "").Replace("-----END RSA PRIVATE KEY-----", "").Replace(Environment.NewLine, "");
                credential = RSACredential;

                // 인증 요청
                var resStartBand = await req.send<StartBandResult>(HttpMethod.Post, BandApiConstants.STARTUP_START_BAND, new Dictionary<string, string>
                {
                    { "public_key", publicKey },
                    { "language", BandApiConstants.LANGUAGE.Substring(0, 2).ToLower() },
                    { "device_id", bandDevice.device_id },
                    { "device_model", bandDevice.model },
                    { "credential", credential }
                });
                string tempEncHmacKey = resStartBand.enc_hmac_key;

                //enc_hmac_key 를 변환하는 작업
                byte[] dec_hmac_key = EZHelper.URLBase64Decode(tempEncHmacKey);
                var rsa = new Pkcs1Encoding(new RsaEngine());
                rsa.Init(false, newKeyPair.Private);
                var decryptedAesKey = rsa.ProcessBlock(dec_hmac_key, 0, dec_hmac_key.Length);
                //base64로 암호화해서 저장(후에 사용하기 편하도록, 깨짐방지)
                string encHmacKey = Convert.ToBase64String(decryptedAesKey);

                //unique_device_id
                bandDevice.di = resStartBand.di;
                //게스트 엑세스 토큰
                bandDevice.guest_access_token = resStartBand.access_token;
                //md값을 만들때 쓰이는 HMAC 키
                bandDevice.enc_hmac_key = encHmacKey;

                return bandDevice;
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

        }
    }
}
