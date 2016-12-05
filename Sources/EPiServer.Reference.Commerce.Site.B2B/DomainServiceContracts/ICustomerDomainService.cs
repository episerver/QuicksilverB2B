using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Enums;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts
{
    public interface ICustomerDomainService
    {
        B2BContact GetCurrentContact();
        List<B2BContact> GetContactsForOrganization(Organization organization);
        void AddContactToOrganization(B2BOrganization organization, B2BContact contact, B2BUserRoles userRole);
        B2BContact GetNewContact();
        B2BContact GetContactById(string contactId);
        IEnumerable<B2BContact> GetContacts();
        CustomerContact GetCustomerByEmail(string email);
    }
}
