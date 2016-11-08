using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Profile.Pages;
using EPiServer.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Features.Profile.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;

namespace EPiServer.Reference.Commerce.Site.Features.Profile.Controllers
{
    [Authorize]
    public class ProfilePageController : PageController<ProfilePage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly ICustomerService _customerService;
        public ProfilePageController(ICustomerService customerService, IContentLoader contentLoader)
        {
            _customerService = customerService;
            _contentLoader = contentLoader;
        }

        public ActionResult Index(ProfilePage currentPage)
        {
            StartPage startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);
            var viewModel = new ProfilePageViewModel
            {
                CurrentPage = currentPage, Contact = _customerService.GetCurrentContact(),
                OrganizationPage = startPage.OrganizationMainPage
            };
            return View(viewModel);
        }
    }
}