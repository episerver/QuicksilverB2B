using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Enums;
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
        CustomerContact GetCustomerByEmail(string email);
    }
}
