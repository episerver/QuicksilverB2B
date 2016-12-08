using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;
using EPiServer.Reference.Commerce.Site.Features.Suborganization.Pages;
using EPiServer.Reference.Commerce.Site.Features.Suborganization.ViewModels;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Mediachase.Commerce.Customers;

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
            //Set selected suborganization
            Session[Constants.Fields.SelectedSuborganization] = Request["suborg"];
            Session[Constants.Fields.SelectedNavSuborganization] = Request["suborg"];

            if (viewModel.SubOrganizationModel == null)
            {
                var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);
                return Redirect(UrlResolver.Current.GetUrl(startPage.OrganizationMainPage));
            }
            viewModel.IsAdmin = CustomerContext.Current.CurrentContact.Properties[Constants.Fields.UserRole].Value.ToString() == Constants.UserRoles.Admin;

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
                //update the locations list
                var updatedLocations = new List<B2BAddressViewModel>();
                foreach (var location in viewModel.SubOrganizationModel.Locations)
                {
                    if (location.Name != "removed")
                    {
                        updatedLocations.Add(location);
                    }
                    else
                    {
                        if (location.AddressId != Guid.Empty)
                        {
                            _addressService.DeleteAddress(viewModel.SubOrganizationModel.OrganizationId.ToString(), location.AddressId.ToString());
                        }
                    }
                }
                viewModel.SubOrganizationModel.Locations = updatedLocations;
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