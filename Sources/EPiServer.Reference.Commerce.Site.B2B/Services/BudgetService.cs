using System.Collections.Generic;
using System.Linq;
using EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(IBudgetService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetDomainService _budgetDomainService;

        public BudgetService(IBudgetDomainService budgetDomainService)
        {
            _budgetDomainService = budgetDomainService;
        }

        public List<BudgetViewModel> GetAllBudgets()
        {
            var budgets = _budgetDomainService.GetAllBudgets();
            return budgets.Select(budget => new BudgetViewModel(budget)).ToList();
        }

        public void CreateNewBudget(BudgetViewModel budgetModel)
        {
            var budget = _budgetDomainService.GetNewBudget();
            budget.Amount = budgetModel.Amount;
            budget.StartDate = budgetModel.StartDate;
            budget.DueDate = budgetModel.DueDate;
            budget.OrganizationId = budgetModel.OrganizationId;
            budget.ContactId = budgetModel.ContactId;
            budget.SaveChanges();
        }

        public void UpdateBudget(BudgetViewModel budgetModel)
        {
            var budget = _budgetDomainService.GetBudgetById(budgetModel.BudgetId);
            budget.Amount = budgetModel.Amount;
            budget.StartDate = budgetModel.StartDate;
            budget.DueDate = budgetModel.DueDate;
            budget.OrganizationId = budgetModel.OrganizationId;
            budget.ContactId = budgetModel.ContactId;
            budget.SaveChanges();
        }
    }
}
