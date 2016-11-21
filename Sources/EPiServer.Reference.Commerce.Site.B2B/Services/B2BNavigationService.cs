using EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Enums;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(IB2BNavigationService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class B2BNavigationService : IB2BNavigationService
    {
        private readonly ICustomerDomainService _customerDomainService;

        public B2BNavigationService(ICustomerDomainService customerDomainService)
        {
            _customerDomainService = customerDomainService;
        }

        public LinkItemCollection FilterB2BNavigationForCurrentUser(LinkItemCollection b2bLinks)
        {
            var filteredLinks = new LinkItemCollection();
            var currentContact = _customerDomainService.GetCurrentContact();

            foreach (var link in b2bLinks)
            {
                switch (currentContact.B2BUserRole)
                {
                    case B2BUserRoles.Admin:
                        if (Constants.B2BNavigationRoles.Admin.Contains(link.Text))
                            filteredLinks.Add(link);
                        break;
                    case B2BUserRoles.Approver:
                        if (Constants.B2BNavigationRoles.Approver.Contains(link.Text))
                            filteredLinks.Add(link);
                        break;
                }
            }
            return filteredLinks;
        }
    }
}
