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
            IsActive = budget.IsActive;
            OrganizationId = budget.OrganizationId;
            ContactId = budget.ContactId;
            BudgetId = budget.BudgetId;
        }

        public BudgetViewModel()
        {
        }
        public int BudgetId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid ContactId { get; set; }
    }
}
