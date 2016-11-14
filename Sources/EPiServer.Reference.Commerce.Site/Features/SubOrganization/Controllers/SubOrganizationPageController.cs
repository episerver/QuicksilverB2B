using System;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;
using EPiServer.Reference.Commerce.Site.Features.Suborganization.Pages;
using EPiServer.Reference.Commerce.Site.Features.Suborganization.ViewModels;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;

namespace EPiServer.Reference.Commerce.Site.Features.Suborganization.Controllers
{
    [Authorize]
    public class SubOrganizationPageController : PageController<SubOrganizationPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IOrganizationService _organizationService;
        private readonly IAddressService _addressService;
        public SubOrganizationPageController(IOrganizationService organizationService, IContentLoader contentLoader, IAddressService addressService)
        {
            _organizationService = organizationService;
            _contentLoader = contentLoader;
            _addressService = addressService;
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
                return Redirect(UrlResolver.Current.GetUrl(startPage.OrganizationMainPage));
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
                return Redirect(UrlResolver.Current.GetUrl(startPage.OrganizationMainPage));
            }

            viewModel.SubOrganizationModel.CountryOptions = _addressService.GetAllCountries();
            return View(viewModel);
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult Save(SubOrganizationPageViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.SubOrganizationModel.Name))
            {
                ModelState.AddModelError("SubOrganization.Name", "SubOrganization Name is requried");
            }

            if (viewModel.SubOrganizationModel.OrganizationId != Guid.Empty)
            {
                _organizationService.UpdateSubOrganization(viewModel.SubOrganizationModel);
            }
            return RedirectToAction("Index", new { suborg = viewModel.SubOrganizationModel.OrganizationId });
        }

        public ActionResult DeleteAddress(SubOrganizationPage currentPage)
        {
            if (Request["suborg"] == null || Request["addressId"] == null)
            {
                var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);
                return Redirect(UrlResolver.Current.GetUrl(startPage.OrganizationMainPage));
            }
            _addressService.DeleteAddress(Request["suborg"], Request["addressId"]);
            return RedirectToAction("Edit", new { suborg = Request["suborg"] });
        }
    }
}