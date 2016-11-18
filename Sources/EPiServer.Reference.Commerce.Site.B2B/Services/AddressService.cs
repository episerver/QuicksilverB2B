using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Orders.Dto;
using Mediachase.Commerce.Orders.Managers;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(IAddressService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class AddressService : IAddressService
    {
        private readonly IOrganizationDomainService _organizationDomainService;
        public AddressService(IOrganizationDomainService organizationDomainService)
        {
            _organizationDomainService = organizationDomainService;
        }

        public void UpdateOrganizationAddress(B2BOrganization organization, B2BAddressViewModel addressModel)
        {
            B2BAddress address = GetOrganizationAddress(organization.OrganizationEntity, addressModel.AddressId) ??
                                 CreateAddress();

            address.OrganizationId = organization.OrganizationId;
            address.Name = addressModel.Name;
            address.Street = addressModel.Street;
            address.City = addressModel.City;
            address.PostalCode = addressModel.PostalCode;
            address.CountryCode = addressModel.CountryCode;
            address.CountryName = GetCountryNameByCode(addressModel.CountryCode);

            address.SaveChanges();
        }

        public IEnumerable<B2BCountryViewModel> GetAllCountries()
        {
            var countries = GetCountries();
            return countries.Country.Select(x => new B2BCountryViewModel { Code = x.Code, Name = x.Name });
        }

        public string GetCountryNameByCode(string code)
        {
            var countryOptions = GetCountries().Country.Select(x => new B2BCountryViewModel { Code = x.Code, Name = x.Name });
            var selectedCountry = countryOptions.FirstOrDefault(x => x.Code == code);
            return selectedCountry?.Name;
        }

        public void DeleteAddress(string organizationId, string addressId)
        {
            var organization = _organizationDomainService.GetOrganizationEntityById(organizationId);
            if (organization == null) return;

            var address = GetOrganizationAddress(organization.OrganizationEntity, new Guid(addressId));
            address?.Address?.Delete();
        }

        private B2BAddress CreateAddress()
        {
            var address = new B2BAddress(CustomerAddress.CreateInstance());
            address.AddressId = BusinessManager.Create(address.Address);
            return address;
        }

        private B2BAddress GetOrganizationAddress(Organization organization, Guid addressId)
        {
            var organizationAddresses = CustomerContext.Current.GetAddressesInOrganization(organization);
            var organizationAddress = organizationAddresses.FirstOrDefault(address => address.AddressId == addressId);
            return organizationAddress != null ? new B2BAddress(organizationAddress) : null;
        }

        private CountryDto GetCountries()
        {
            return CountryManager.GetCountries();
        }
    }
}
