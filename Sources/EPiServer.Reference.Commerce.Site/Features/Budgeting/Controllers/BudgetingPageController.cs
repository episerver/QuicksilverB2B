using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Logging;
using EPiServer.Reference.Commerce.Site.B2B.Filters;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Budgeting.Pages;
using EPiServer.Reference.Commerce.Site.Features.Budgeting.ViewModels;
using EPiServer.Web.Mvc;
using Mediachase.Commerce;

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

        [NavigationAuthorize("Admin,Approver")]
        public ActionResult Index(BudgetingPage currentPage)
        {
           List<BudgetViewModel> organizationBudgets = new List<BudgetViewModel>();
           List<BudgetViewModel> suborganizationsBudgets = new List<BudgetViewModel>();

           var currentOrganization = !string.IsNullOrEmpty(Session["SelectedSuborganization"]?.ToString()) 
                ? _organizationService.GetSubOrganizationById(Session["SelectedSuborganization"].ToString()) 
                : _organizationService.GetCurrentUserOrganization();

            var viewModel = new BudgetingPageViewModel
            {
                CurrentPage = currentPage,
                OrganizationBudgets = organizationBudgets
            };

            viewModel.IsSubOrganization = !string.IsNullOrEmpty(Session["SelectedSuborganization"]?.ToString());

            var currentBudget = _budgetService.GetCurrentOrganizationBudget(currentOrganization.OrganizationId);
            if (currentBudget != null) viewModel.CurrentBudgetViewModel = new BudgetViewModel(currentBudget);

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

        [NavigationAuthorize("Admin")]
        public ActionResult NewBudget(DateTime startDateTime, DateTime finishDateTime, decimal amount, string currency, string status)
        {
            var success = true;
            try
            {
                var currentOrganization = _organizationService.GetCurrentUserOrganization();
                var organizationId = currentOrganization.OrganizationId;

                //if (!string.IsNullOrEmpty(Session["SelectedSuborganization"]?.ToString()))
                //{
                //    organizationId = Guid.Parse(Session["SelectedSuborganization"].ToString());
                //    // Validate Ammount of available budget.
                //    if (!_budgetService.IsValidBudgetAmount(currentOrganization.OrganizationId, amount))
                //        return Json(new { success = false });
                //}
                // Invalid date selection. Overlaps with another budget.
                if (!_budgetService.IsValidTimeLine(startDateTime, finishDateTime, organizationId))
                    return Json(new { success = false });

                _budgetService.CreateNewBudget(new BudgetViewModel
                {
                    Amount = amount,
                    SpentBudget = 0,
                    Currency = currency,
                    StartDate = startDateTime,
                    DueDate = finishDateTime,
                    OrganizationId = organizationId,
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

        [NavigationAuthorize("Admin")]
        public ActionResult UpdateBudget(DateTime startDateTime, DateTime finishDateTime, decimal amount, string currency, string status, int budgetId)
        {
            // edit acces rules for user type also check if bugdet order is at the user organization
            // check if it current budget
            var success = true;
            try
            {
                _budgetService.UpdateBudget( new BudgetViewModel
                {
                    Amount = amount,
                    StartDate = startDateTime,
                    DueDate = finishDateTime,
                    Status = status,
                    Currency = currency,
                    BudgetId = budgetId
                });
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                success = false;
            }

            return Json(new { result = success });
        }


        [NavigationAuthorize("Admin")]
        public ActionResult EditBudget(BudgetingPage currentPage, int budgetId)
        {
            var currentOrganization = _organizationService.GetCurrentUserOrganization();
            var currentBudget = _budgetService.GetCurrentOrganizationBudget(currentOrganization.OrganizationId);
           
            var viewModel = new BudgetingPageViewModel
            {
                CurrentPage = currentPage,
                NewBudgetOption = new BudgetViewModel(_budgetService.GetBudgetById(budgetId))
            };
            if (currentBudget != null && currentBudget.BudgetId == budgetId) viewModel.NewBudgetOption.IsCurrentBudget = true;

            return View(viewModel);
        }

        [NavigationAuthorize("Admin")]
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