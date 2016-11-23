using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Logging;
using EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Filters;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Budgeting.Pages;
using EPiServer.Reference.Commerce.Site.Features.Budgeting.ViewModels;
using EPiServer.Web.Mvc;
using Mediachase.Commerce;
using Mediachase.Commerce.Customers;
using NuGet;

namespace EPiServer.Reference.Commerce.Site.Features.Budgeting.Controllers
{
    [Authorize]
    public class BudgetingPageController : PageController<BudgetingPage>
    {
        private readonly IBudgetService _budgetService;
        private readonly IOrganizationService _organizationService;
        private readonly ICurrentMarket _currentMarket;

        public BudgetingPageController(IBudgetService budgetService, IOrganizationService organizationService, ICurrentMarket currentMarket)
        {
            _budgetService = budgetService;
            _organizationService = organizationService;
            _currentMarket = currentMarket;
        }

        [NavigationAuthorize("Admin,Approver,None")]
        public ActionResult Index(BudgetingPage currentPage)
        {
           List<BudgetViewModel> organizationBudgets = new List<BudgetViewModel>();
           List<BudgetViewModel> suborganizationsBudgets = new List<BudgetViewModel>();
           var currentOrganization = _organizationService.GetCurrentUserOrganization();
           var viewModel = new BudgetingPageViewModel
            {
                CurrentPage = currentPage,
                OrganizationBudgets = organizationBudgets
            };
            var budgets = _budgetService.GetOrganizationBudgets(currentOrganization.OrganizationId);
            if (budgets != null)
            {
                organizationBudgets.AddRange(budgets.Select(budget => new BudgetViewModel(budget) { OrganizationName = currentOrganization.Name }));
                viewModel.OrganizationBudgets = organizationBudgets;
            }

            var suborganizations = currentOrganization.SubOrganizations;
            if (suborganizations != null)
                foreach (var suborganization in suborganizations)
                {
                  var suborgBudgets = _budgetService.GetOrganizationBudgets(suborganization.OrganizationId);
                  if (suborgBudgets != null)
                  {
                      suborganizationsBudgets.AddRange(suborgBudgets.Select(suborgBudget => new BudgetViewModel(suborgBudget) { OrganizationName = suborganization.Name }));
                  }
                  viewModel.SubOrganizationsBudgets = suborganizationsBudgets;
                }
            
            return View(viewModel);
        }

        [NavigationAuthorize("Admin,Approver,None")]
        public ActionResult NewBudget(DateTime startDateTime, DateTime finisDateTime, decimal ammount, string currency, string status)
        {
            var success = true;
            try
            {
                var currentOrganization = _organizationService.GetCurrentUserOrganization();
                var currentUserId = CustomerContext.Current.CurrentContactId;
                _budgetService.CreateNewBudget(new BudgetViewModel
                {
                    Amount = ammount,
                    SpentBudget = 0,
                    Currency = currency,
                    StartDate = startDateTime,
                    DueDate = finisDateTime,
                    OrganizationId = currentOrganization.OrganizationId,
                    ContactId = currentUserId,
                    IsActive = true,
                    Status = status
                });
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                success = false;
            }
           
            return Json(new {success = success});
        }

        [NavigationAuthorize("Admin,Approver,None")]
        public ActionResult UpdateBudget(DateTime startDateTime, DateTime finisDateTime, decimal allocatedAmmount, string currency, string status, int budgetId)
        {
            // edit acces rules for user type
            var success = true;
            try
            {
                _budgetService.UpdateBudget( new BudgetViewModel
                {
                    Amount = allocatedAmmount,
                    StartDate = startDateTime,
                    DueDate = finisDateTime,
                    Status = status,
                    Currency = currency
                });
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                success = false;
            }

            return Json(new { result = success });
        }


        [NavigationAuthorize("Admin,Approver,None")]
        public ActionResult EditBudget(BudgetingPage currentPage)
        {
            var viewModel = new BudgetingPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }

        [NavigationAuthorize("Admin,Approver,None")]
        public ActionResult AddBudget(BudgetingPage currentPage)
        {
            var viewModel = new BudgetingPageViewModel{CurrentPage = currentPage};
            var availableCurrencies = _currentMarket.GetCurrentMarket().Currencies as List<Currency>;
            if (availableCurrencies != null)
            {
                var currencies = new List<string>();
                currencies.AddRange(availableCurrencies.Select(currency => currency.CurrencyCode));
                viewModel.AvailableCurrencies = currencies;
            }
            return View(viewModel);
        }
    }
}