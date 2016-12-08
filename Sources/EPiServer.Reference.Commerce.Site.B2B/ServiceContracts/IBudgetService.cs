using System;
using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;

namespace EPiServer.Reference.Commerce.Site.B2B.ServiceContracts
{
    public interface IBudgetService
    {
        List<BudgetViewModel> GetAllBudgets();
        void CreateNewBudget(BudgetViewModel budgetModel);
        List<Budget> GetOrganizationBudgets(Guid organizationId);
        void UpdateBudget(BudgetViewModel budgetModel);
        Budget GetBudgetById(int budgetId);
        bool IsTimeOverlapped(DateTime startDate, DateTime dueDateTime, Guid organizationGuid);
        Budget GetCurrentOrganizationBudget(Guid organizationId);
        bool HasEnoughAmount(Guid organizationGuid, decimal amount, DateTime startDateTime, DateTime finishDateTime);
        bool HasEnoughAmountOnCurrentBudget(Guid organizationGuid, decimal amount);
        List<Budget> GetOrganizationPurchasersBudgets(Guid organizationId);
        List<Budget> GetOrganizationBudgetsWithoutPurchasers(Guid organizationId);
        Budget GetCustomerCurrentBudget(Guid organizationId, Guid purchaserGuid);

        bool IsSuborganizationValidTimeSlice(DateTime startDateTime, DateTime finishDateTime, Guid organizationGuid);
        Budget GetBudgetByTimeLine(Guid organizationId, DateTime startDate, DateTime endDate);
        bool LockOrganizationAmount(DateTime startDate, DateTime endDate, Guid guid, decimal amount);
        bool UnLockOrganizationAmount(DateTime startDate, DateTime endDate, Guid guid, decimal amount);
        bool CheckAmount(Guid organizationGuid, decimal newLockAmount, decimal unlockAmount);
        bool LockUserAmount(DateTime startDate, DateTime endDate, Guid organizationGuid, Guid userGuid, decimal amount);

        bool CheckAmountByTimeLine(Guid organizationGuid, decimal newLockAmount, DateTime startDateTime, DateTime finishDateTime);
        bool ValidateSuborganizationNewAmount(Guid organizationGuid, Guid parentOrganizationId, decimal newLockAmount);
    }
}
