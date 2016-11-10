using System;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;

namespace EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels
{
    public class BudgetViewModel
    {
        public BudgetViewModel(Budget budget)
        {
            StartDate = budget.StartDate;
            DueDate = budget.DueDate;
            Amount = budget.Amount;
        }

        public BudgetViewModel()
        {
        }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public float Amount { get; set; }
    }
}
