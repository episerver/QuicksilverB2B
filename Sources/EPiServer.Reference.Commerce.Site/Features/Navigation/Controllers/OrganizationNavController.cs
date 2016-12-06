using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Navigation.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;

namespace EPiServer.Reference.Commerce.Site.Features.Navigation.Controllers
{
    public class OrganizationNavController : Controller
    {
        private readonly IOrganizationService _organizationService;
        private readonly ICustomerService _customerService;
        private readonly IContentLoader _contentLoader;

        public OrganizationNavController(IOrganizationService organizationService, ICustomerService customerService, IContentLoader contentLoader)
        {
            _organizationService = organizationService;
            _customerService = customerService;
            _contentLoader = contentLoader;
        }

        // GET: OrganizationNav
        public ActionResult OrgNavigation()
        {
            var model = new OrgNavigationViewModel
            {
                Organization = _customerService.CanSeeOrganizationNav()
                    ? _organizationService.GetCurrentUserOrganization()
                    : null,
                CurrentOrganization = !string.IsNullOrEmpty(Session[Constants.Fields.SelectedNavSuborganization]?.ToString())
                    ? _organizationService.GetSubOrganizationById(Session[Constants.Fields.SelectedNavSuborganization].ToString())
                    : _organizationService.GetCurrentUserOrganization()
            };

            StartPage startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);
            if (startPage != null)
            {
                model.OrganizationPage = startPage.OrganizationMainPage;
                model.SubOrganizationPage = startPage.SubOrganizationPage;
            }

            return View("~/Views/Shared/_OrgNavigation.cshtml", model);
        }
    }
}