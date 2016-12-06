using System;
using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Extensions;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.BusinessFoundation.Data.Meta.Management;
using Mediachase.Commerce;

namespace EPiServer.Reference.Commerce.Site.B2B.Models.Entities
{
    public class Budget
    {
        public Budget(EntityObject budgetEntity)
        {
            BudgetEntity = budgetEntity;
        }

        public List<Currency> BudgetCurrencies;

        public EntityObject BudgetEntity { get; set; }
        public int BudgetId
        {
            get { return BudgetEntity.PrimaryKeyId ?? 0; }
            set { BudgetEntity.PrimaryKeyId = value; }
        }
        public DateTime StartDate
        {
            get { return BudgetEntity.GetDateTimeValue(Constants.Fields.StartDate); }
            set { BudgetEntity[Constants.Fields.StartDate] = value; }
        }
        public DateTime DueDate
        {
            get { return BudgetEntity.GetDateTimeValue(Constants.Fields.DueDate); }
            set { BudgetEntity[Constants.Fields.DueDate] = value; }
        }
        public decimal Amount
        {
            get { return BudgetEntity.GetDecimalValue(Constants.Fields.Amount); }
            set { BudgetEntity[Constants.Fields.Amount] = value; }
        }

        public decimal SpentBudget
        {
            get { return BudgetEntity.GetDecimalValue(Constants.Fields.SpentBudget); }
            set { BudgetEntity[Constants.Fields.SpentBudget] = value; }
        }
        public string Currency
        {
            get { return BudgetEntity.GetStringValue(Constants.Fields.Currency); }
            set { BudgetEntity[Constants.Fields.Currency] = value; }
        }

        public decimal LockAmount
        {
            get { return BudgetEntity.GetDecimalValue(Constants.Fields.LockAmount); }
            set { BudgetEntity[Constants.Fields.LockAmount] = value; }
        }

        public Guid OrganizationId
        {
            get { return BudgetEntity.GetGuidValue(MetaClassManager.GetPrimaryKeyName(Constants.Classes.Organization)); }
            set { BudgetEntity[MetaClassManager.GetPrimaryKeyName(Constants.Classes.Organization)] = new PrimaryKeyId(value); }
        }
        public Guid ContactId
        {
            get { return BudgetEntity.GetGuidValue(MetaClassManager.GetPrimaryKeyName(Constants.Classes.Contact)); }
            set { BudgetEntity[MetaClassManager.GetPrimaryKeyName(Constants.Classes.Contact)] = new PrimaryKeyId(value); }
        }
        public string Status
        {
            get { return BudgetEntity.GetStringValue(Constants.Fields.Status); }
            set { BudgetEntity[Constants.Fields.Status] = value; }
        }
        public string PurchaserName
        {
            get { return BudgetEntity.GetStringValue(Constants.Fields.PurchaserName); }
            set { BudgetEntity[Constants.Fields.PurchaserName] = value; }
        }

        public bool IsActive => StartDate <= DateTime.Now && DueDate > DateTime.Now;

        public decimal RemainingBudget => Amount - SpentBudget;
        public decimal UnallocatedBudget => Amount - LockAmount;

        public void SaveChanges()
        {
            BusinessManager.Update(BudgetEntity);
        }
    }
}
