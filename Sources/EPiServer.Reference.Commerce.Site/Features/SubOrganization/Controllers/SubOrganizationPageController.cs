using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;
using EPiServer.Reference.Commerce.Site.Features.Suborganization.Pages;
using EPiServer.Reference.Commerce.Site.Features.Suborganization.ViewModels;
using EPiServer.Shell;
using EPiServer.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.Suborganization.Controllers
{
    [Authorize]
    public class SubOrganizationPageController : PageController<SubOrganizationPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IOrganizationService _organizationService;
        public SubOrganizationPageController(IOrganizationService organizationService, IContentLoader contentLoader)
        {
            _organizationService = organizationService;
            _contentLoader = contentLoader;
        }

        public ActionResult Index(SubOrganizationPage currentPage)
        {
            var viewModel = new SubOrganizationPageViewModel
            {
                CurrentPage = currentPage,
                SubOrganizationModel = _organizationService.GetSubOrganizationById(Request["suborg"])
            };

            if (viewModel.SubOrganizationModel == null)
            {
                var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);
                return Redirect(startPage.OrganizationMainPage.GetUri().ToString());
            }

            return View(viewModel);
        }

        public ActionResult Edit(SubOrganizationPage currentPage)
        {
            var viewModel = new SubOrganizationPageViewModel { CurrentPage = currentPage,
                SubOrganizationModel = _organizationService.GetSubOrganizationById(Request["suborg"])
            };

            if (viewModel.SubOrganizationModel == null)
            {
                var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);
                return Redirect(startPage.OrganizationMainPage.GetUri().ToString());
            }
            return View(viewModel);
        }
    }
}