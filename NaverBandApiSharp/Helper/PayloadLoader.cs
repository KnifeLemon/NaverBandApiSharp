using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NaverBandApiSharp.Helper
{
    public static class PayloadLoader
    {
        private static string RESOURCE_BASE_NAME = "NaverBandApiSharp.Payloads";

        public static bool Exists(string fileNameWithExtension)
        {
            var assembly = Assembly.GetExecutingAssembly();
            //var names = assembly.GetManifestResourceNames();
            var stream = assembly.GetManifestResourceStream($"{RESOURCE_BASE_NAME}.{fileNameWithExtension}");
            if (stream == null)
                return false;

            return true;

        }

        public static JObject Get(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream($"{RESOURCE_BASE_NAME}.{name}");
            if (stream == null)
                return null;
            using (var reader = new System.IO.StreamReader(stream))
                {
                string result = reader.ReadToEnd();
                return JObject.Parse(result);
            }
        }
    }
}
