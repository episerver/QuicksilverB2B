using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.B2B.ServiceContracts
{
    public interface ICustomerService
    {
        ContactViewModel GetCurrentContact();
        List<ContactViewModel> GetContactsForCurrentOrganization();
        void CreateUser(ContactViewModel contact, string contactId);
        ContactViewModel GetContactById(string id);
        void EditContact(ContactViewModel model);
        void RemoveContact(string id);
        bool CanSeeOrganizationNav();
        void AddContactToOrganization(string contactId, string organizationId = null);
        void AddContactToOrganization(B2BContact contact, string organizationId = null);
        void UpdateContact(string contactId, string userRole, string location = null);
        bool HasOrganization(string contactId);
        CustomerContact GetCustomerByEmail(string email);
        List<ContactViewModel> GetContactsByOrganizationId(string organizationId);
    }
}
