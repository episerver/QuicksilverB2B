using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Enums;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.ServiceLocation;
using Mediachase.BusinessFoundation.Data.Business;
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

        public B2BContact GetContactById(string contactId)
        {
            if (string.IsNullOrEmpty(contactId)) return null;

            var id = new Guid(contactId);
            var contact = CustomerContext.Current.GetContactById(id);
            return contact != null ? new B2BContact(contact) : null;
        }

        public void AddContactToOrganization(B2BOrganization organization, B2BContact contact, B2BUserRoles userRole)
        {
            contact.B2BOrganization = organization;
            contact.UserRole = userRole.ToString();
            contact.SaveChanges();
        }

        public B2BContact GetNewContact()
        {
            var contact = new B2BContact(CustomerContact.CreateInstance());
            contact.ContactId = BusinessManager.Create(contact.Contact);
            return contact;
        }

        public IEnumerable<B2BContact> GetContacts()
            => CustomerContext.Current.GetContacts().Select(c => new B2BContact(c));

        public CustomerContact GetCustomerByEmail(string email)
        {
            return CustomerContext.Current.GetContacts().FirstOrDefault(user => user.Email == email);
        }
    }
}
