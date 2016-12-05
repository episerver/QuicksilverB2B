using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.Navigation.ViewModels
{
    public class OrgNavigationViewModel
    {
        public OrganizationModel Organization { get; set; }
        public OrganizationModel CurrentOrganization { get; set; }

        public ContentReference OrganizationPage { get; set; }

        public ContentReference SubOrganizationPage { get; set; }
    }
}