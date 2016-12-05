using System;
using System.Linq;
using EPiServer.Reference.Commerce.Site.B2B.Adapters;
using EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Enums;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(IOrganizationService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationDomainService _organizationDomainService;
        private readonly ICustomerDomainService _customerDomainService;
        private readonly IAddressService _addressService;

        public OrganizationService(IOrganizationDomainService organizationDomainService,
            ICustomerDomainService customerDomainService, IAddressService addressService)
        {
            _organizationDomainService = organizationDomainService;
            _customerDomainService = customerDomainService;
            _addressService = addressService;
        }

        public OrganizationModel GetCurrentUserOrganization()
        {
            var currentOrganization = _organizationDomainService.GetCurrentUserOrganizationEntity();
            if (currentOrganization == null) return null;

            if (currentOrganization.ParentOrganizationId == Guid.Empty) return new OrganizationModel(currentOrganization);

            var parentOrganization =
                _organizationDomainService.GetOrganizationEntityById(currentOrganization.ParentOrganizationId.ToString());
            return new OrganizationModel(currentOrganization)
            {
                ParentOrganization = new OrganizationModel(parentOrganization)
            };
        }

        public SubOrganizationModel GetSubOrganizationById(string subOrganizationId)
        {
            var subOrganization = _organizationDomainService.GetOrganizationEntityById(subOrganizationId);
            if (subOrganization == null) return null;

            if (subOrganization.ParentOrganizationId == Guid.Empty) return new SubOrganizationModel(subOrganization);

            var parentOrganization =
                _organizationDomainService.GetOrganizationEntityById(subOrganization.ParentOrganizationId.ToString());
            return new SubOrganizationModel(subOrganization)
            {
                ParentOrganization = new OrganizationModel(parentOrganization)
            };
        }

        public void UpdateSubOrganization(SubOrganizationModel subOrganizationModel)
        {
            var organization =
                _organizationDomainService.GetOrganizationEntityById(subOrganizationModel.OrganizationId.ToString());
            organization.Name = subOrganizationModel.Name;
            organization.SaveChanges();
            foreach (var location in subOrganizationModel.Locations)
            {
                _addressService.UpdateOrganizationAddress(organization, location);
            }
        }

        public void CreateOrganization(OrganizationModel organizationInfo)
        {
            var organization = _organizationDomainService.GetNewOrganization();
            organization.Name = organizationInfo.Name;
            organization.SaveChanges();

            _customerDomainService.AddContactToOrganization(organization, _customerDomainService.GetCurrentContact(),
                B2BUserRoles.Admin);
            _addressService.UpdateOrganizationAddress(organization, organizationInfo.Address);
        }

        public void UpdateOrganization(OrganizationModel organizationInfo)
        {
            var organization =
                _organizationDomainService.GetOrganizationEntityById(organizationInfo.OrganizationId.ToString());
            organization.Name = organizationInfo.Name;
            organization.SaveChanges();
            _addressService.UpdateOrganizationAddress(organization, organizationInfo.Address);
        }

        public void CreateSubOrganization(SubOrganizationModel newSubOrganization)
        {
            var currentOrganization = _organizationDomainService.GetCurrentUserOrganizationEntity();
            if (currentOrganization == null) return;

            var organization = _organizationDomainService.GetNewOrganization();
            organization.Name = newSubOrganization.Name;
            organization.ParentOrganizationId = currentOrganization.OrganizationId;
            organization.SaveChanges();

            foreach (var location in newSubOrganization.Locations)
            {
                _addressService.UpdateOrganizationAddress(organization, location);
            }
        }

        public string GetUserCurrentOrganizationLocation()
        {
            var currentOrganization = _organizationDomainService.GetCurrentUserOrganizationEntity();
            if (currentOrganization == null) return string.Empty;

            return B2BAdapters.MarketCodeAdapter(currentOrganization.Addresses.FirstOrDefault().CountryCode);
        }

    }
}
