using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.Reference.Commerce.Site.B2B.Models.Pages;
using EPiServer.Reference.Commerce.Site.Features.Shared.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.Organization.ViewModels
{
    public class OrganizationPageViewModel : PageViewModel<OrganizationPage>
    {
        public B2BOrganization Organization { get; set; }
    }
}