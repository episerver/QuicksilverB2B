using EPiServer.Reference.Commerce.Site.B2B.Enums;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;

namespace EPiServer.Reference.Commerce.Site.B2B.ServiceContracts
{
    public interface ICustomerService
    {
        B2BContact GetCurrentContact();
        void AddContactToOrganization(B2BOrganization organization, B2BContact contact, B2BUserRoles userRole);
    }
}
