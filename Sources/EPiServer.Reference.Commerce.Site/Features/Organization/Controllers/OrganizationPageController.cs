using System;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.Filters;
using EPiServer.Reference.Commerce.Site.B2B.Models.Pages;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Organization.ViewModels;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using EPiServer.Web.Mvc;
using EPiServer.Reference.Commerce.Site.B2B;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;

namespace EPiServer.Reference.Commerce.Site.Features.Organization.Controllers
{
    [Authorize]
    public class OrganizationPageController : PageController<OrganizationPage>
    {
        private readonly IOrganizationService _organizationService;
        private readonly IAddressService _addressService;
        private readonly IBudgetService _budgetService;
        private readonly IContentLoader _contentLoader;

        public OrganizationPageController(IOrganizationService organizationService, IAddressService addressService, IBudgetService budgetService, IContentLoader contentLoader)
        {
            _organizationService = organizationService;
            _addressService = addressService;
            _budgetService = budgetService;
            _contentLoader = contentLoader;
        }

        public ActionResult Index(OrganizationPage currentPage)
        {
            //Clear selected suborganization
            Session[Constants.Fields.SelectedSuborganization] = "";

            if (Request.QueryString["showForm"] != null && bool.Parse(Request.QueryString["showForm"]))
            {
                return RedirectToAction("Create");
            }
            var viewModel = new OrganizationPageViewModel
            {
                CurrentPage = currentPage,
                Organization = _organizationService.GetCurrentUserOrganization()
            };

            StartPage startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);
            if (startPage != null)
            {
                viewModel.SubOrganizationPage = startPage.SubOrganizationPage;
            }

            if (viewModel.Organization != null && viewModel.Organization?.Address == null)
            {
                viewModel.Organization.Address = new B2BAddressViewModel();
            }
            if (viewModel.Organization == null)
            {
                return RedirectToAction("Edit");
            }

            if (viewModel.Organization?.SubOrganizations != null)
            {
                var suborganizations = viewModel.Organization.SubOrganizations;
                var organizationIndex = 0;
                foreach (var suborganization in suborganizations)
                {
                    var budget = _budgetService.GetCurrentOrganizationBudget(suborganization.OrganizationId);
                    if (budget != null)
                    viewModel.Organization.SubOrganizations.ElementAt(organizationIndex).CurrentBudgetViewModel = 
                        new BudgetViewModel(budget);
                    organizationIndex ++;
                }
            }
            
            return View(viewModel);
        }

        [NavigationAuthorize("Admin,None")]
        public ActionResult Create(OrganizationPage currentPage)
        {
            OrganizationPageViewModel viewModel = new OrganizationPageViewModel
            {
                Organization = new OrganizationModel
                {
                    Address = new B2BAddressViewModel
                    {
                        CountryOptions = _addressService.GetAllCountries()
                    }
                },
                CurrentPage = currentPage
            };
            return View("Edit",viewModel);
        }

        [NavigationAuthorize("Admin")]
        public ActionResult Edit(OrganizationPage currentPage, string organizationId)
        {
            OrganizationPageViewModel viewModel = new OrganizationPageViewModel
            {
                Organization = _organizationService.GetCurrentUserOrganization() ?? new OrganizationModel(),
                CurrentPage = currentPage
            };
            if (viewModel.Organization?.Address != null)
            {
                viewModel.Organization.Address.CountryOptions = _addressService.GetAllCountries();
            }
            else
            {
                if (viewModel.Organization != null)
                {
                    viewModel.Organization.Address = new B2BAddressViewModel
                    {
                        CountryOptions = _addressService.GetAllCountries()
                    };
                }
            }
            return View(viewModel);
        }

        [NavigationAuthorize("Admin")]
        public ActionResult AddSub(OrganizationPage currentPage)
        {
            var viewModel = new OrganizationPageViewModel
            {
                CurrentPage = currentPage,
                Organization = _organizationService.GetCurrentUserOrganization() ?? new OrganizationModel(),
                NewSubOrganization = new SubOrganizationModel()
                {
                    CountryOptions = _addressService.GetAllCountries()
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [AllowDBWrite]
        [NavigationAuthorize("Admin,None")]
        public ActionResult Save(OrganizationPageViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Organization.Name))
            {
                ModelState.AddModelError("Organization.Name", "Organization Name is requried");
            }

            if (viewModel.Organization.OrganizationId == Guid.Empty)
            {
                _organizationService.CreateOrganization(viewModel.Organization);
            }
            else
            {
                _organizationService.UpdateOrganization(viewModel.Organization);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AllowDBWrite]
        [NavigationAuthorize("Admin")]
        public ActionResult SaveSub(OrganizationPageViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.NewSubOrganization.Name))
            {
                ModelState.AddModelError("NewSubOrganization.Name", "Sub organization Name is requried");
            }

            _organizationService.CreateSubOrganization(viewModel.NewSubOrganization);
            return RedirectToAction("Index");
        }
    }
}