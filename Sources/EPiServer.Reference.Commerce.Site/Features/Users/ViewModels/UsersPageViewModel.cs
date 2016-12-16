using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Models.Pages;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Shared.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.Users.ViewModels
{
    public class UsersPageViewModel : PageViewModel<UsersPage>
    {
        public List<ContactViewModel> Users { get; set; }
        public ContactViewModel Contact { get; set; }
        public List<OrganizationModel> Organizations { get; set; }
        public SubOrganizationModel SubOrganization { get; set; }
    }
}