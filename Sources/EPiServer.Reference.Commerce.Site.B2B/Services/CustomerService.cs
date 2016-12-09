using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Find.Helpers.Text;
using EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Enums;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(ICustomerService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class CustomerService : ICustomerService
    {
        private readonly IOrganizationDomainService _organizationDomainService;
        private readonly ICustomerDomainService _customerDomainService;

        public CustomerService(IOrganizationDomainService organizationDomainService,
            ICustomerDomainService customerDomainService)
        {
            _organizationDomainService = organizationDomainService;
            _customerDomainService = customerDomainService;
        }

        public ContactViewModel GetCurrentContact()
        {
            var currentContact = _customerDomainService.GetCurrentContact();
            return currentContact?.Contact != null ? new ContactViewModel(currentContact) : new ContactViewModel();
        }

        public CustomerContact GetCustomerByEmail(string email)
        {
           return _customerDomainService.GetCustomerByEmail(email);
        }

        public ContactViewModel GetContactById(string id)
        {
            return new ContactViewModel(_customerDomainService.GetContactById(id));
        }

        public List<ContactViewModel> GetContactsForCurrentOrganization()
        {
            var currentOrganization = _organizationDomainService.GetCurrentUserOrganizationEntity();
            if (currentOrganization == null) return new List<ContactViewModel>();

            var organizationUsers = _customerDomainService.GetContactsForOrganization(currentOrganization.OrganizationEntity);

            if (currentOrganization.SubOrganizations.Count > 0)
            {
                foreach (var subOrg in currentOrganization.SubOrganizations)
                {
                    var contacts = _customerDomainService.GetContactsForOrganization(subOrg.OrganizationEntity);
                    organizationUsers.AddRange(contacts);   
                }
            }

            return organizationUsers.Select(user => new ContactViewModel(user)).ToList();
        }

        public List<ContactViewModel> GetContactsByOrganizationId(string organizationId)
        {
            var currentOrganization = _organizationDomainService.GetOrganizationEntityById(organizationId);
            if (currentOrganization == null) return new List<ContactViewModel>();

            var organizationUsers = _customerDomainService.GetContactsForOrganization(currentOrganization.OrganizationEntity);

            if (currentOrganization.SubOrganizations.Count > 0)
            {
                foreach (var subOrg in currentOrganization.SubOrganizations)
                {
                    var contacts = _customerDomainService.GetContactsForOrganization(subOrg.OrganizationEntity);
                    organizationUsers.AddRange(contacts);
                }
            }

            return organizationUsers.Select(user => new ContactViewModel(user)).ToList();
        }

        public void CreateUser(ContactViewModel contactModel, string contactId)
        {
            var contact = new B2BContact(CustomerContact.CreateInstance())
            {
                ContactId = new Guid(contactId),
                FirstName = contactModel.FirstName,
                LastName = contactModel.LastName,
                Email = contactModel.Email,
                UserId = contactModel.Email,
                UserRole = contactModel.UserRole,
                FullName = contactModel.FullName,
                UserLocationId = (contactModel.UserRole != B2BUserRoles.Admin.ToString()) ? contactModel.Location : ""
            };
            contact.SaveChanges();

            if (contactModel.UserRole == B2BUserRoles.Admin.ToString())
                AddContactToOrganization(contact);
            else
                AddContactToOrganization(contact, contactModel.OrganizationId);
        }

        public void EditContact(ContactViewModel model)
        {
            UpdateContact(model.ContactId.ToString(), model.UserRole, model.Location);
        }

        public void RemoveContact(string id)
        {
            var contact = _customerDomainService.GetContactById(id);
            contact.B2BOrganization = new B2BOrganization(new Organization());
            contact.SaveChanges();
        }

        public void AddContactToOrganization(string contactId, string organizationId = null)
        {
            var contact = _customerDomainService.GetContactById(contactId);
            contact.B2BOrganization = organizationId.IsNullOrEmpty()
                ? _organizationDomainService.GetCurrentUserOrganizationEntity()
                : _organizationDomainService.GetOrganizationEntityById(organizationId);
            contact.SaveChanges();
        }
        public void AddContactToOrganization(B2BContact contact, string organizationId = null)
        {
            contact.B2BOrganization = organizationId.IsNullOrEmpty()
                ? _organizationDomainService.GetCurrentUserOrganizationEntity()
                : _organizationDomainService.GetOrganizationEntityById(organizationId);
            contact.SaveChanges();
        }

        public void UpdateContact(string contactId, string userRole, string location = null)
        {
            var contact = _customerDomainService.GetContactById(contactId);
            contact.UserRole = userRole;
            contact.UserLocationId = location;
            contact.SaveChanges();
        }

        public bool CanSeeOrganizationNav()
        {
            var currentRole = _customerDomainService.GetCurrentContact().B2BUserRole;
            return currentRole == B2BUserRoles.Admin || currentRole == B2BUserRoles.Approver;
        }

        public bool HasOrganization(string contactId)
        {
            var contact = _customerDomainService.GetContactById(contactId);
            return contact.B2BOrganization != null;
        }
    }
}
