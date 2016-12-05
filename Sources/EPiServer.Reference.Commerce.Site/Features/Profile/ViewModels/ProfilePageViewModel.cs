using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Profile.Pages;
using EPiServer.Reference.Commerce.Site.Features.Shared.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.Profile.ViewModels
{
    public class ProfilePageViewModel : PageViewModel<ProfilePage>
    {
        public ContactViewModel Contact { get; set; }
        public ContentReference OrganizationPage { get; set; }
        public BudgetViewModel CurrentPersonalBudget { get; set; }
    }
}