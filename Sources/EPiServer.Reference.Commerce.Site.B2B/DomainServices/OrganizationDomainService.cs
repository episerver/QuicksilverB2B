using EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.ServiceLocation;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.B2B.DomainServices
{
    [ServiceConfiguration(typeof(IOrganizationDomainService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class OrganizationDomainService : IOrganizationDomainService
    {
        public B2BOrganization GetCurrentUserOrganizationEntity()
        {
            return CustomerContext.Current.CurrentContact != null && CustomerContext.Current.CurrentContact.ContactOrganization != null
                ? new B2BOrganization(CustomerContext.Current.CurrentContact.ContactOrganization)
                : null;
        }

        public B2BOrganization GetOrganizationEntityById(string organizationId)
        {
            if (string.IsNullOrEmpty(organizationId)) return null;

            var organization = CustomerContext.Current.GetOrganizationById(organizationId);
            return organization != null ? new B2BOrganization(organization) : null;
        }

        public B2BOrganization GetNewOrganization()
        {
            var organization = new B2BOrganization(Organization.CreateInstance());
            organization.OrganizationId = BusinessManager.Create(organization.OrganizationEntity);
            return organization;
        }
    }
}
