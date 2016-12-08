using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.Models.Pages;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Shared.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.Organization.ViewModels
{
    public class OrganizationPageViewModel : PageViewModel<OrganizationPage>
    {
        public OrganizationModel Organization { get; set; }
        public SubOrganizationModel NewSubOrganization { get; set; }
        public ContentReference SubOrganizationPage { get; set; }
        public bool IsAdmin { get; set; }
    }
}