using EPiServer.Reference.Commerce.Site.B2B.Enums;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(ICustomerService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class CustomerService : ICustomerService
    {
        public B2BContact GetCurrentContact()
        {
            return new B2BContact(CustomerContext.Current.CurrentContact);
        }

        public void AddContactToOrganization(B2BOrganization organization, B2BContact contact, B2BUserRoles userRole)
        {
            contact.B2BOrganization = organization;
            contact.UserRole = userRole.ToString();
            contact.SaveChanges();
        }
    }
}
