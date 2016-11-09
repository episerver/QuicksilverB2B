using EPiServer.Reference.Commerce.Site.B2B.Enums;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(IOrganizationService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class OrganizationService : IOrganizationService
    {
        private readonly ICustomerService _customerService;
        private readonly IAddressService _addressService;
        public OrganizationService(ICustomerService customerService, IAddressService addressService)
        {
            _customerService = customerService;
            _addressService = addressService;
        }

        public OrganizationModel GetCurrentUserOrganization()
        {
            var currentOrganization = GetCurrentUserOrganizationEntity();
            return currentOrganization != null ? new OrganizationModel(currentOrganization) : null;
        }
        
        public void CreateOrganization(OrganizationModel organizationInfo)
        {
            var organization = new B2BOrganization(Organization.CreateInstance()) {Name = organizationInfo.Name};
            organization.OrganizationId = BusinessManager.Create(organization.OrganizationEntity);

            _customerService.AddContactToOrganization(organization, _customerService.GetCurrentContact(), B2BUserRoles.Admin);
            _addressService.UpdateOrganizationAddress(organization, organizationInfo.Address);
        }

        public void UpdateOrganization(OrganizationModel organizationInfo)
        {
            var organization = GetOrganizationEntityById(organizationInfo.OrganizationId.ToString());
            organization.Name = organizationInfo.Name;
            organization.SaveChanges();
            _addressService.UpdateOrganizationAddress(organization, organizationInfo.Address);
        }

        private B2BOrganization GetCurrentUserOrganizationEntity()
        {
            return CustomerContext.Current.CurrentContact.ContactOrganization != null
                ? new B2BOrganization(CustomerContext.Current.CurrentContact.ContactOrganization)
                : null;
        }

        private B2BOrganization GetOrganizationEntityById(string organizationId)
        {
            if (string.IsNullOrEmpty(organizationId)) return null;

            var organization = CustomerContext.Current.GetOrganizationById(organizationId);
            return organization != null ? new B2BOrganization(organization) : null;
        }
    }
}
