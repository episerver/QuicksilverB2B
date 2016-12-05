using System;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
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
        private readonly IBudgetService _budgetService;
        public ProfilePageController(ICustomerService customerService, IContentLoader contentLoader, IBudgetService budgetService)
        {
            _customerService = customerService;
            _contentLoader = contentLoader;
            _budgetService = budgetService;
        }

        public ActionResult Index(ProfilePage currentPage)
        {
            StartPage startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);
            var viewModel = new ProfilePageViewModel
            {
                CurrentPage = currentPage, Contact = _customerService.GetCurrentContact(),
                OrganizationPage = startPage.OrganizationMainPage
            };
            var userOrganization = _customerService.GetCurrentContact().Organization;
            Budget currentPersonalBudget = null;
            if (userOrganization != null && userOrganization.OrganizationId != Guid.Empty)
                currentPersonalBudget = _budgetService.GetCustomerCurrentBudget(userOrganization.OrganizationId,
                    _customerService.GetCurrentContact().ContactId);
            if (currentPersonalBudget != null)
                viewModel.CurrentPersonalBudget = new BudgetViewModel(currentPersonalBudget);
            return View(viewModel);
        }
    }
}