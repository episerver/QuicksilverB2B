using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Logging;
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

        public bool LockOrganizationAmount(DateTime startDate, DateTime endDate, Guid guid, decimal amount)
        {
            try
            {
                var deductBudget = GetBudgetByTimeLine(guid, startDate, endDate);
                UpdateBudget(new BudgetViewModel
                {
                    Amount = deductBudget.Amount,
                    OrganizationId = deductBudget.OrganizationId,
                    Currency = deductBudget.Currency,
                    SpentBudget = deductBudget.SpentBudget,
                    DueDate = deductBudget.DueDate,
                    StartDate = deductBudget.StartDate,
                    Status = deductBudget.Status,
                    IsActive = deductBudget.IsActive,
                    BudgetId = deductBudget.BudgetId,
                    ContactId = deductBudget.ContactId,
                    PurchaserName = deductBudget.PurchaserName,
                    LockAmount = deductBudget.LockAmount + amount
                });

            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                return false;
            }
            return true;
        }


        public bool LockUserAmount(DateTime startDate, DateTime endDate, Guid organizationGuid,Guid userGuid, decimal amount)
        {
            try
            {
                var deductBudget = GetCustomerCurrentBudget(organizationGuid, userGuid);
                UpdateBudget(new BudgetViewModel
                {
                    Amount = deductBudget.Amount,
                    OrganizationId = deductBudget.OrganizationId,
                    Currency = deductBudget.Currency,
                    SpentBudget = deductBudget.SpentBudget,
                    DueDate = deductBudget.DueDate,
                    StartDate = deductBudget.StartDate,
                    Status = deductBudget.Status,
                    IsActive = deductBudget.IsActive,
                    BudgetId = deductBudget.BudgetId,
                    ContactId = deductBudget.ContactId,
                    PurchaserName = deductBudget.PurchaserName,
                    LockAmount = deductBudget.LockAmount + amount
                });

            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                return false;
            }
            return true;
        }


        public bool UnLockOrganizationAmount(DateTime startDate, DateTime endDate, Guid guid, decimal amount)
        {
            try
            {
                var deductBudget = GetBudgetByTimeLine(guid, startDate, endDate);
                UpdateBudget(new BudgetViewModel
                {
                    Amount = deductBudget.Amount,
                    OrganizationId = deductBudget.OrganizationId,
                    Currency = deductBudget.Currency,
                    SpentBudget = deductBudget.SpentBudget,
                    DueDate = deductBudget.DueDate,
                    StartDate = deductBudget.StartDate,
                    Status = deductBudget.Status,
                    IsActive = deductBudget.IsActive,
                    BudgetId = deductBudget.BudgetId,
                    ContactId = deductBudget.ContactId,
                    PurchaserName = deductBudget.PurchaserName,
                    LockAmount = deductBudget.LockAmount - amount
                });

            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                return false;
            }
            return true;
        }

        private void UpdateBudgetEntity(Budget budgetEntity, BudgetViewModel budgetModel)
        {
            budgetEntity.Amount = budgetModel.Amount;
            budgetEntity.Currency = budgetModel.Currency;
            budgetEntity.StartDate = budgetModel.StartDate;
            budgetEntity.DueDate = budgetModel.DueDate;
            budgetEntity.Status = budgetModel.Status;
            budgetEntity.PurchaserName = budgetModel.PurchaserName;
            budgetEntity.LockAmount = budgetModel.LockAmount;
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

        public Budget GetBudgetById(int budgetId)
        {
            return _budgetDomainService.GetBudgetById(budgetId);
        }

        public bool IsTimeOverlapped(DateTime startDate, DateTime dueDateTime, Guid organizationGuid)
        {
            var budgets = _budgetDomainService.GetOrganizationBudgets(organizationGuid);
            if (budgets == null || budgets.Count == 0) return true;
            if (budgets.Any(budget => (DateTime.Compare(budget.StartDate, dueDateTime) <= 0) &&
                                      (DateTime.Compare(startDate, budget.DueDate) <= 0)))
            {
                return false;
            }

            return true;
        }

        public bool IsSuborganizationValidTimeSlice(DateTime startDateTime, DateTime finishDateTime, Guid organizationGuid)
        {
            var budgets = _budgetDomainService.GetOrganizationBudgets(organizationGuid);
            if (budgets == null || budgets.Count == 0) return false;
            if (budgets.Any(budget => (DateTime.Compare(budget.StartDate, startDateTime) <= 0) &&
                                      (DateTime.Compare(finishDateTime, budget.DueDate) <= 0) && 
                                      (DateTime.Compare(budget.StartDate, finishDateTime) <= 0) &&
                                      (DateTime.Compare(startDateTime, budget.DueDate) <= 0)
                                      ))
            {
                return true;
            }

            return false;

        }

        public Budget GetCurrentOrganizationBudget(Guid organizationId)
        {
            return _budgetDomainService.GetCurrentOrganizationBudget(organizationId);
        }

        public bool HasEnoughAmount(Guid organizationGuid, decimal amount, DateTime startDateTime, DateTime finishDateTime)
        {
            var currentBudget = GetBudgetByTimeLine(organizationGuid,startDateTime,finishDateTime);
            if (currentBudget == null) return false;

            return (currentBudget.Amount - currentBudget.SpentBudget - currentBudget.LockAmount - amount) >=0 ;
        }

        public bool HasEnoughAmountOnCurrentBudget(Guid organizationGuid, decimal amount)
        {
            var currentBudget = _budgetDomainService.GetCurrentOrganizationBudget(organizationGuid);
            if (currentBudget == null) return false;

            return (currentBudget.Amount - currentBudget.SpentBudget - currentBudget.LockAmount - amount) >= 0;
        }

        public bool CheckAmount(Guid organizationGuid, decimal newLockAmount, decimal unlockAmount)
        {
            var currentBudget = _budgetDomainService.GetCurrentOrganizationBudget(organizationGuid);
            if (currentBudget == null) return false;

            return (currentBudget.Amount + unlockAmount - currentBudget.SpentBudget - currentBudget.LockAmount - newLockAmount) >= 0;
        }

        public bool ValidateSuborganizationNewAmount(Guid organizationGuid, Guid parentOrganizationId, decimal newLockAmount)
        {
            var currentBudget = _budgetDomainService.GetCurrentOrganizationBudget(organizationGuid);
            if (currentBudget == null) return false;
            var parentCurrentBudget = _budgetDomainService.GetCurrentOrganizationBudget(parentOrganizationId);
            if (parentCurrentBudget == null) return false;

            return (newLockAmount <= parentCurrentBudget.UnallocatedBudget + currentBudget.Amount) && (newLockAmount >= currentBudget.LockAmount);
        }

        public bool CheckAmountByTimeLine(Guid organizationGuid, decimal newLockAmount, DateTime startDateTime, DateTime finishDateTime)
        {
            var currentBudget = GetBudgetByTimeLine(organizationGuid, startDateTime, finishDateTime);
            if (currentBudget == null) return false;

            return (currentBudget.Amount - currentBudget.LockAmount - newLockAmount) >= 0;
        }

        public List<Budget> GetOrganizationPurchasersBudgets(Guid organizationId)
        {
            return _budgetDomainService.GetOrganizationPurchasersBudgets(organizationId);
        }

        public List<Budget> GetOrganizationBudgetsWithoutPurchasers(Guid organizationId)
        {
            return _budgetDomainService.GetOrganizationBudgets(organizationId);
        }

       public  Budget GetCustomerCurrentBudget(Guid organizationId, Guid purchaserGuid)
        {
            return _budgetDomainService.GetCustomerCurrentBudget(organizationId, purchaserGuid);
        }

        public Budget GetBudgetByTimeLine(Guid organizationId, DateTime startDate, DateTime endDate)
        {
            var organizationBudgets = _budgetDomainService.GetOrganizationBudgets(organizationId);
            if (!organizationBudgets.Any()) return null;
            var returnBudget = organizationBudgets.Where(budget => (DateTime.Compare(budget.StartDate, endDate) <= 0) &&
                                      (DateTime.Compare(startDate, budget.DueDate) <= 0));
            if (!returnBudget.Any()) return null;
            return returnBudget.FirstOrDefault();
        }
    }
}
