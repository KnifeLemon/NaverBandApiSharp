using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NaverBandApiSharp.API;
using NaverBandApiSharp.Classes.Models;

namespace NaverBandApiSharp.Helper
{
    internal class BandDeviceHelper
    {
        public static BandDevice MakeAndroidDevice(MobileDevice device)
        {
            //Mozilla/5.0 (Linux; {Android Version}; {Build Tag etc.}) AppleWebKit/{WebKit Rev} (KHTML, like Gecko) Chrome/{Chrome Rev} Mobile Safari/{WebKit Rev} - Mobile
            //Mozilla/5.0 (Linux; {Android Version}; {Build Tag etc.}) AppleWebKit/{WebKit Rev} (KHTML, like Gecko) Chrome/{Chrome Rev} Safari/{WebKit Rev} - tablet
            return new BandDevice()
            {
                os_version = device.os_version,
                api_level = device.api_level,
                device = $"{device.brand} {device.model}",
                brand = device.brand,
                model = device.model,
                device_id = MakeBandDeviceID(device),
                web_user_agent = $"Mozilla/5.0 (Linux; Android {device.os_version}; {device.model}) {device.chrome_version}",
                band_user_agent = string.Format(BandApiConstants.USER_AGENT, BandApiConstants.APP_VERSION, device.os_version, device.brand, device.model)
            };
        }

        /**
         * <summary>
         *      디바이스 ID 생성
         * </summary>
         */
        private static string MakeBandDeviceID(MobileDevice device)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            string input = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, 16).Select(x => input[rand.Next(0, input.Length)]);
            string android_id = new string(chars.ToArray());

            /*
            JAVA Code
            public final String a() {
                StringBuffer B1 = a.B1("35");
                B1.append(Build.BOARD.length() % 10);
                B1.append(Build.BRAND.length() % 10);
                B1.append(Build.CPU_ABI.length() % 10);
                B1.append(Build.DEVICE.length() % 10);
                B1.append(Build.DISPLAY.length() % 10);
                B1.append(Build.HOST.length() % 10);
                B1.append(Build.MANUFACTURER.length() % 10);
                B1.append(Build.MODEL.length() % 10);
                B1.append(Build.PRODUCT.length() % 10);
                B1.append(Build.TAGS.length() % 10);
                B1.append(Build.TYPE.length() % 10);
                B1.append(Build.USER.length() % 10);
                return B1.toString();
            }
             */

            string os = device.os_version;
            string brand = device.brand;
            string model = device.model;

            StringBuilder sb = new StringBuilder();
            sb.Append("35");
            sb.Append(brand.Length % 10); //Build.BOARD
            sb.Append(brand.Length % 10); //Build.BRAND
            sb.Append("x86".Length % 10); //Build.CPU_ABI
            sb.Append(model.Length % 10); //Build.DEVICE
            sb.Append(model.Length % 10); //Build.DISPLAY
            sb.Append($"{brand}-user {os} 20171130.276299 release-keys".Length % 10); //Build.DISPLAY
            sb.Append("se.infra".Length % 10); //Build.HOST
            sb.Append(brand.Length % 10); //Build.MANUFACTURER
            sb.Append(model.Length % 10); //Build.MODEL
            sb.Append(model.Length % 10); //Build.PRODUCT
            sb.Append("release-keys".Length % 10); //Build.TAGS
            sb.Append("user".Length % 10); //Build.TYPE
            sb.Append(brand.Length % 10); //Build.USER
            string str = sb.ToString();
            string str2 = sb + android_id;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(str2));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return "M_" + builder.ToString();
            }
        }
    }
}
