using System;
using System.Linq;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(IAddressService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class AddressService : IAddressService
    {
        public void UpdateOrganizationAddress(B2BOrganization organization, B2BAddressViewModel addressModel)
        {
            B2BAddress address = GetOrganizationAddress(organization.OrganizationEntity, addressModel.AddressId) ??
                                 CreateAddress();

            address.OrganizationId = organization.OrganizationId;
            address.Name = addressModel.Name;
            address.Line1 = addressModel.Line1;

            address.SaveChanges();
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
    }
}
