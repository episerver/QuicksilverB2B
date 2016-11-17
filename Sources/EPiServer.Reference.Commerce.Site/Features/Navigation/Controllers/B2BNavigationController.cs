using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.Features.Navigation.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;
using EPiServer.SpecializedProperties;
using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;

namespace EPiServer.Reference.Commerce.Site.Features.Navigation.Controllers
{
    public class B2BNavigationController : Controller
    {
        private readonly IContentLoader _contentLoader;
        private readonly IOrganizationService _organizationService;
        private readonly IB2BNavigationService _b2bNavigationService;

        public B2BNavigationController(IContentLoader contentLoader, IOrganizationService organizationService, IB2BNavigationService b2bNavigationService)
        {
            _contentLoader = contentLoader;
            _organizationService = organizationService;
            _b2bNavigationService = b2bNavigationService;
        }

        public ActionResult Index(IContent currentContent)
        {
            var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);
           
            var viewModel = new NavigationViewModel
            {
                StartPage = startPage,
                CurrentContentLink = currentContent?.ContentLink,
                UserLinks = new LinkItemCollection()
            };

            var organization = _organizationService.GetCurrentUserOrganization();
            if (organization == null)
            {
                return PartialView(viewModel);
            }

            if (startPage.B2BMenu != null)
            {
                viewModel.UserLinks.AddRange(_b2bNavigationService.FilterB2BNavigationForCurrentUser(startPage.B2BMenu));
            }

            return PartialView(viewModel);
        }
    }
}