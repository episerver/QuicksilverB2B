using System;
using EPiServer.Reference.Commerce.Site.B2B.Extensions;
using Mediachase.BusinessFoundation.Data.Business;

namespace EPiServer.Reference.Commerce.Site.B2B.Models.Entities
{
    public class Budget
    {
        public Budget(EntityObject budgetEntity)
        {
            BudgetEntity = budgetEntity;
        }
        public EntityObject BudgetEntity { get; set; }
        public DateTime StartDate {
            get { return BudgetEntity.GetDateTimeValue(Constants.Fields.StartDate); }
            set
            {
                BudgetEntity[Constants.Fields.StartDate] = value;
            }
        }
        public DateTime DueDate
        {
            get { return BudgetEntity.GetDateTimeValue(Constants.Fields.DueDate); }
            set { BudgetEntity[Constants.Fields.DueDate] = value; }
        }
        public float Amount
        {
            get { return BudgetEntity.GetFloatValue(Constants.Fields.Amount); }
            set { BudgetEntity[Constants.Fields.Amount] = value; }
        }

        public void SaveChanges()
        {
            BusinessManager.Update(BudgetEntity);
        }
    }
}
