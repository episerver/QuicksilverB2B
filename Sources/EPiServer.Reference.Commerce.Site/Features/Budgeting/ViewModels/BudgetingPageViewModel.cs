using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Budgeting.Pages;
using EPiServer.Reference.Commerce.Site.Features.Shared.ViewModels;
using Mediachase.Commerce;

namespace EPiServer.Reference.Commerce.Site.Features.Budgeting.ViewModels
{
    public class BudgetingPageViewModel : PageViewModel<BudgetingPage>
    {
        public List<BudgetViewModel> OrganizationBudgets { get; set; }
        public List<BudgetViewModel> SubOrganizationsBudgets { get; set; }
        public BudgetViewModel NewBudgetOption { get; set; }
        public List<string> AvailableCurrencies { get; set; } 
    }
}