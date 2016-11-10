using System.Collections.Generic;
using System.Linq;
using EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Enums;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.B2B.DomainServices
{
    [ServiceConfiguration(typeof(ICustomerDomainService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class CustomerDomainService : ICustomerDomainService
    {
        public B2BContact GetCurrentContact()
        {
            return new B2BContact(CustomerContext.Current.CurrentContact);
        }

        public List<B2BContact> GetContactsForOrganization(Organization organization)
        {
            var organizationContacts = CustomerContext.Current.GetCustomerContactsInOrganization(organization)?.ToList();
            return organizationContacts != null && organizationContacts.Any()
                ? organizationContacts.Select(contact => new B2BContact(contact)).ToList()
                : new List<B2BContact>();
        }

        public void AddContactToOrganization(B2BOrganization organization, B2BContact contact, B2BUserRoles userRole)
        {
            contact.B2BOrganization = organization;
            contact.UserRole = userRole.ToString();
            contact.SaveChanges();
        }
    }
}
