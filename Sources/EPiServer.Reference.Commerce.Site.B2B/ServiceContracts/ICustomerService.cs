using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;

namespace EPiServer.Reference.Commerce.Site.B2B.ServiceContracts
{
    public interface ICustomerService
    {
        ContactViewModel GetCurrentContact();
        List<ContactViewModel> GetContactsForCurrentOrganization();
    }
}
