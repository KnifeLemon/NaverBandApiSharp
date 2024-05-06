using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaverBandApiSharp.Classes.Lists
{
    public static class PLMNList
    {
        public static List<PLMN> Get()
        {
            return new List<PLMN>
            {
                new PLMN
                {
                    Name = "KT",
                    MCC = "450",
                    MNC = "02"
                },
                new PLMN
                {
                    Name = "SKT",
                    MCC = "450",
                    MNC = "05"
                },
                new PLMN
                {
                    Name = "LGU PLus",
                    MCC = "450",
                    MNC = "06",
                },
            };
        }
    }

    [Serializable]
    public class PLMN
    {
        public string Name { get; set; }
        public string MCC { get; set; }
        public string MNC { get; set; }

        static Random r = new Random();
        public override string ToString()
        {
            return MCC + MNC;
        }

        public static string GetRandomPLMN()
        {
            List<PLMN> plmns = PLMNList.Get();
            int index = r.Next(plmns.Count);
            return plmns[index].ToString();
        }
    }
}
