using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.SpecializedProperties;

namespace EPiServer.Reference.Commerce.Site.B2B.ServiceContracts
{
    public interface IB2BNavigationService
    {
        LinkItemCollection FilterB2BNavigationForCurrentUser(LinkItemCollection b2bLinks);
    }
}
