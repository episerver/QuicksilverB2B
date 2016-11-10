using System.Collections.Generic;
using System.Linq;
using EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(ICustomerService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class CustomerService : ICustomerService
    {
        private readonly IOrganizationDomainService _organizationDomainService;
        private readonly ICustomerDomainService _customerDomainService;

        public CustomerService(IOrganizationDomainService organizationDomainService,
            ICustomerDomainService customerDomainService)
        {
            _organizationDomainService = organizationDomainService;
            _customerDomainService = customerDomainService;
        }

        public ContactViewModel GetCurrentContact()
        {
            return new ContactViewModel(_customerDomainService.GetCurrentContact());
        }

        public List<ContactViewModel> GetContactsForCurrentOrganization()
        {
            var currentOrganization = _organizationDomainService.GetCurrentUserOrganizationEntity();
            if (currentOrganization == null) return new List<ContactViewModel>();

            var organizationUsers = _customerDomainService.GetContactsForOrganization(currentOrganization.OrganizationEntity);
            return organizationUsers.Select(user => new ContactViewModel(user)).ToList();
        }
    }
}
