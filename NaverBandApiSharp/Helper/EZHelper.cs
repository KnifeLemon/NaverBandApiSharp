using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaverBandApiSharp.Helper
{
    public static class EZHelper
    {
        public static string GetRandomString(int length, string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")
        {
            return new string(Enumerable.Repeat(chars, length).Select(s => s[new Random(Guid.NewGuid().GetHashCode()).Next(s.Length)]).ToArray());
        }

        public static int Random(int min, int max)
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            return r.Next(min, max);
        }

        public static string UpperCaseUrlEncode(string s)
        {
            char[] temp = System.Web.HttpUtility.UrlEncode(s).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }
            return new string(temp);
        }

        public static byte[] URLBase64Decode(string signature)
        {
            string incoming = signature.ToString().Replace('_', '/').Replace('-', '+');
            switch (signature.ToString().Length % 4)
            {
                case 2:
                    incoming += "==";
                    break;
                case 3:
                    incoming += "=";
                    break;
            }

            return Convert.FromBase64String(incoming);
        }
    }
}
