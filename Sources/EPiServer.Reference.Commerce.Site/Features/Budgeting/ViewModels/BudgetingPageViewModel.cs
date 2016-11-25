using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Budgeting.Pages;
using EPiServer.Reference.Commerce.Site.Features.Shared.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.Budgeting.ViewModels
{
    public class BudgetingPageViewModel : PageViewModel<BudgetingPage>
    {
        public List<BudgetViewModel> OrganizationBudgets { get; set; }
        public List<BudgetViewModel> SubOrganizationsBudgets { get; set; }
        public List<BudgetViewModel> PurchasersSpendingLimits { get; set; }
        public BudgetViewModel NewBudgetOption { get; set; }
        public List<string> AvailableCurrencies { get; set; } 
        public BudgetViewModel CurrentBudgetViewModel { get; set; }
        public bool IsSubOrganization { get; set; }
        public bool IsAdmin { get; set; }
    }
}