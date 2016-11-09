using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;
using EPiServer.Reference.Commerce.Site.Features.Profile.Pages;
using EPiServer.Reference.Commerce.Site.Features.Shared.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.Profile.ViewModels
{
    public class ProfilePageViewModel : PageViewModel<ProfilePage>
    {
        public B2BContact Contact { get; set; }
        public ContentReference OrganizationPage { get; set; }
    }
}