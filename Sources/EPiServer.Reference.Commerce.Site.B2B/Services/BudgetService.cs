using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
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
            UpdateBudgetEntity(budget, budgetModel);
        }

        public void UpdateBudget(BudgetViewModel budgetModel)
        {
            var budget = _budgetDomainService.GetBudgetById(budgetModel.BudgetId);
            UpdateBudgetEntity(budget, budgetModel);
        }


        private void UpdateBudgetEntity(Budget budgetEntity, BudgetViewModel budgetModel)
        {
            budgetEntity.Amount = budgetModel.Amount;
            budgetEntity.Currency = budgetModel.Currency;
            budgetEntity.StartDate = budgetModel.StartDate;
            budgetEntity.DueDate = budgetModel.DueDate;
            budgetEntity.Status = budgetModel.Status;
            if (budgetModel.OrganizationId != Guid.Empty)
            {
                budgetEntity.OrganizationId = budgetModel.OrganizationId;
            }
            if (budgetModel.ContactId != Guid.Empty)
            {
                budgetEntity.ContactId = budgetModel.ContactId;
            }
            budgetEntity.SaveChanges();
        }

        public List<Budget> GetOrganizationBudgets(Guid organizationId)
        {
            return _budgetDomainService.GetOrganizationBudgets(organizationId);
        }
    }
}
