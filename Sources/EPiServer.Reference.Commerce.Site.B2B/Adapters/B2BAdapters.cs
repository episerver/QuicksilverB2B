using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiServer.Reference.Commerce.Site.B2B.Adapters
{
    public static class B2BAdapters
    {
        public static string MarketCodeAdapter(string countryCode)
        {
            switch (countryCode)
            {
                case "USA":
                    return "US";
                case "ES":
                    return "ESP";
                default:
                    return "US";
            }
        }
    }
}
