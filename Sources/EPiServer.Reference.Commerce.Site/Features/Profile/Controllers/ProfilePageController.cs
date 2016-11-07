using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Profile.Pages;
using EPiServer.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Features.Profile.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.Profile.Controllers
{
    [Authorize]
    public class ProfilePageController : PageController<ProfilePage>
    {
        private readonly ICustomerService _customerService;
        public ProfilePageController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public ActionResult Index(ProfilePage currentPage)
        {
            var viewModel = new ProfilePageViewModel { CurrentPage = currentPage, Contact = _customerService.GetCurrentContact() };
            return View(viewModel);
        }
    }
}