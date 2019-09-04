using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IceCreamRPAExample
{
    public class RecipeModel
    {
        public string IceCream1 { get; set; }
        public string IceCream2 { get; set; }
        public string IceCream3 { get; set; }

        public string MixIn1 { get; set; }
        public string MixIn2 { get; set; }
        public string MixIn3 { get; set; }

        public List<string> GetIceCreamFlavors()
        {
            List<string> flavors = new List<string>();
            if(this.IceCream1 != null)
            {
                flavors.Add(IceCream1);
            }
            if (this.IceCream2 != null)
            {
                flavors.Add(IceCream2);
            }
            if (this.IceCream3 != null)
            {
                flavors.Add(IceCream3);
            }

            return flavors;
        }

        public List<string> GetMixins()
        {
            List<string> mixIns = new List<string>();
            if(this.MixIn1 != null)
            {
                mixIns.Add(this.MixIn1);
            }
            if (this.MixIn2 != null)
            {
                mixIns.Add(this.MixIn2);
            }
            if (this.MixIn3 != null)
            {
                mixIns.Add(this.MixIn3);
            }
            return mixIns;
        }

    }
}
