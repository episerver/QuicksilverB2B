using System;
using EPiServer.Reference.Commerce.Site.B2B.Extensions;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.BusinessFoundation.Data.Meta.Management;

namespace EPiServer.Reference.Commerce.Site.B2B.Models.Entities
{
    public class Budget
    {
        public Budget(EntityObject budgetEntity)
        {
            BudgetEntity = budgetEntity;
        }
        public EntityObject BudgetEntity { get; set; }
        public Guid BudgetId
        {
            get { return BudgetEntity.PrimaryKeyId ?? Guid.Empty; }
            set { BudgetEntity.PrimaryKeyId = (PrimaryKeyId?)value; }
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
        public Guid OrganizationId
        {
            get { return BudgetEntity.GetGuidValue(MetaClassManager.GetPrimaryKeyName(Constants.Classes.Organization)); }
            set { BudgetEntity[MetaClassManager.GetPrimaryKeyName(Constants.Classes.Organization)] = value; }
        }
        public Guid ContactId
        {
            get { return BudgetEntity.GetGuidValue(MetaClassManager.GetPrimaryKeyName(Constants.Classes.Contact)); }
            set { BudgetEntity[MetaClassManager.GetPrimaryKeyName(Constants.Classes.Contact)] = value; }
        }

        public bool IsActive => StartDate <= DateTime.Now && DueDate > DateTime.Now;

        public void SaveChanges()
        {
            BusinessManager.Update(BudgetEntity);
        }
    }
}
