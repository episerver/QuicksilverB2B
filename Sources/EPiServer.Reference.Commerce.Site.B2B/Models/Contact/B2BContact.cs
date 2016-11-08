using EPiServer.Reference.Commerce.Site.B2B.Extensions;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.B2B.Models.Contact
{
    public class B2BContact
    {
        public B2BContact(CustomerContact contact)
        {
            Contact = contact;
        }

        public CustomerContact Contact { get; }
        public string FirstName { get { return Contact.FirstName; } set { Contact.FirstName = value; } }
        public string LastName { get { return Contact.LastName; } set { Contact.LastName = value; } }
        public string FullName { get { return Contact.FullName; } set { Contact.FullName = value; } }
        public string Email { get { return Contact.Email; } set { Contact.Email = value; } }
        public string UserRole
        {
            get { return Contact.GetStringValue(Constants.Fields.UserRole); }
            set { Contact.Properties[Constants.Fields.UserRole].Value = value; }
        }

        public B2BOrganization B2BOrganization {
            get { return Contact.ContactOrganization != null ? new B2BOrganization(Contact.ContactOrganization) : null; }
            set { Contact.OwnerId = value.OrganizationEntity.PrimaryKeyId; }
        }

        public void SaveChanges()
        {
            Contact.SaveChanges();
        }
    }
}
