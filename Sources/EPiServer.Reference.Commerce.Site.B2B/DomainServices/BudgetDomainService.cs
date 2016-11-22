using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.ServiceLocation;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;

namespace EPiServer.Reference.Commerce.Site.B2B.DomainServices
{
    [ServiceConfiguration(typeof(IBudgetDomainService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class BudgetDomainService : IBudgetDomainService
    {
        public List<Budget> GetActiveUserBudgets(Guid contactId)
        {
            var budgets = GetUserBudgets(contactId);
            if (budgets == null || !budgets.Any()) return null;

            return budgets.Where(budget => budget.IsActive).ToList();
        }
        public List<Budget> GetActiveOrganizationBudgets(Guid organizationId)
        {
            var budgets = GetOrganizationBudgets(organizationId);
            if (budgets == null || !budgets.Any()) return null;

            return budgets.Where(budget => budget.IsActive).ToList();
        }

        public List<Budget> GetUserBudgets(Guid contactId)
        {
            var budgets = GetAllBudgets();
            if (budgets == null || !budgets.Any()) return null;

            return budgets.Where(budget => budget.ContactId == contactId).ToList();
        }

        public List<Budget> GetOrganizationBudgets(Guid organizationId)
        {
            var budgets = GetAllBudgets();
            if (budgets == null || !budgets.Any()) return null;

            return budgets.Where(budget => budget.OrganizationId == organizationId).ToList();
        }

        public List<Budget> GetAllBudgets()
        {
            var budgets =  BusinessManager.List(Constants.Classes.Budget, new List<FilterElement>().ToArray());
            return budgets?.Select(budget => new Budget(budget)).ToList();
        }

        public Budget GetBudgetById(int budgetId)
        {
            var budget = BusinessManager.Load(Constants.Classes.Budget, new PrimaryKeyId(budgetId));
            return budget != null ? new Budget(budget) : null;
        }

        public Budget GetNewBudget()
        {
            var budgetEntity = BusinessManager.InitializeEntity(Constants.Classes.Budget);
            budgetEntity.PrimaryKeyId = BusinessManager.Create(budgetEntity);
            var budget = new Budget(budgetEntity);
            budget.SaveChanges();
            return budget;
        }
    }
}
