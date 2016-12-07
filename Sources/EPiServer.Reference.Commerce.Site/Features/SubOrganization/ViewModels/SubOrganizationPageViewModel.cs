using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Shared.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Suborganization.Pages;

namespace EPiServer.Reference.Commerce.Site.Features.Suborganization.ViewModels
{
    public class SubOrganizationPageViewModel : PageViewModel<SubOrganizationPage>
    {
        public SubOrganizationModel SubOrganizationModel { get; set; }
        public bool IsAdmin { get; set; }
    }
}