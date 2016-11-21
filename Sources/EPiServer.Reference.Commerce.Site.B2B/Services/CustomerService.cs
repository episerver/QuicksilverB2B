using System;
using System.Collections.Generic;
using System.Linq;
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
            return new ContactViewModel(_customerDomainService.GetCurrentContact());
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

            var organization = _organizationDomainService.GetOrganizationEntityById(contactModel.OrganizationId);
            contact.B2BOrganization = organization;
            contact.SaveChanges();
        }

        public void EditContact(ContactViewModel model)
        {
            var contact = _customerDomainService.GetContactById(model.ContactId.ToString());
            contact.UserRole = model.UserRole;
            contact.UserLocationId = model.Location;
            contact.SaveChanges();
        }

        public void RemoveContact(string id)
        {
            var contact = _customerDomainService.GetContactById(id);
            contact.B2BOrganization = new B2BOrganization(new Organization());
            contact.SaveChanges();
        }

        public bool CanSeeOrganizationNav()
        {
            var currentRole = _customerDomainService.GetCurrentContact().B2BUserRole;
            return currentRole == B2BUserRoles.Admin || currentRole == B2BUserRoles.Approver;
        }
    }
}
