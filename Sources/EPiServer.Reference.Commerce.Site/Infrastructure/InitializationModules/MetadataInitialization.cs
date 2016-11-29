using System.Linq;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Reference.Commerce.Site.B2B;
using Mediachase.BusinessFoundation;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Meta.Management;
using Mediachase.BusinessFoundation.MetaForm;

namespace EPiServer.Reference.Commerce.Site.Infrastructure.InitializationModules
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    public class MetadataInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            InitializeCustomerMetaData();
        }

        private void InitializeCustomerMetaData()
        {
            var budgetMetaClass = GetOrCreateMetaClass(Constants.Classes.Budget, Constants.Classes.BudgetFriendly);
            CreateRelation(Constants.Classes.Organization, Constants.Classes.Budget);
            CreateRelation(Constants.Classes.Contact, Constants.Classes.Budget);

            CreateMetaField(budgetMetaClass, Constants.Fields.StartDate, Constants.Fields.StartDateFriendly, MetaFieldType.Date);
            CreateMetaField(budgetMetaClass, Constants.Fields.DueDate, Constants.Fields.DueDateFriendly, MetaFieldType.Date);
            CreateMetaField(budgetMetaClass, Constants.Fields.Amount, Constants.Fields.Amount, MetaFieldType.Currency);
            CreateMetaField(budgetMetaClass, Constants.Fields.Currency, Constants.Fields.Currency, MetaFieldType.Text);
            CreateMetaField(budgetMetaClass, Constants.Fields.Status, Constants.Fields.Status, MetaFieldType.Text);
            CreateMetaField(budgetMetaClass, Constants.Fields.SpentBudget, Constants.Fields.SpentBudget, MetaFieldType.Currency);
            CreateMetaField(budgetMetaClass, Constants.Fields.PurchaserName, Constants.Fields.PurchaserName, MetaFieldType.Text);
            CreateMetaField(budgetMetaClass, Constants.Fields.LockAmount, Constants.Fields.LockAmount, MetaFieldType.Currency);

            var contactMetaClass = GetOrCreateMetaClass(Constants.Classes.Contact, Constants.Classes.Contact);
            CreateMetaField(contactMetaClass, Constants.Fields.UserRole, Constants.Fields.UserRoleFriendly, MetaFieldType.Text);
            CreateMetaField(contactMetaClass, Constants.Fields.UserLocation, Constants.Fields.UserLocationFriendly, MetaFieldType.Text);
        }
        
        private MetaClass GetOrCreateMetaClass(string metaClassName, string metaClassFriendlyName)
        {
            return
                DataContext.Current.MetaModel.MetaClasses.Cast<MetaClass>()
                    .FirstOrDefault(mc => mc.Name == metaClassName) ??
                DataContext.Current.MetaModel.CreateMetaClass(metaClassName, metaClassFriendlyName);
        }

        private void CreateMetaField(MetaClass metaClass, string metaFieldName, string metaFieldFriendlyName, string type)
        {
            if (metaClass.Fields[metaFieldName] == null)
            {
                metaClass.CreateMetaField(metaFieldName, metaFieldFriendlyName, type, true, "", new AttributeCollection());
                FormController.AddMetaPrimitive(metaClass.Name, Constants.Forms.EditForm, metaFieldName);
                FormController.AddMetaPrimitive(metaClass.Name, Constants.Forms.ShortInfoForm, metaFieldName);
                FormController.AddMetaPrimitive(metaClass.Name, Constants.Forms.ViewForm, metaFieldName);
            }
        }

        private void CreateRelation(string primaryClassName, string relatedClassName)
        {
            MetaClass relatedMetaClass = DataContext.Current.GetMetaClass(relatedClassName);
            MetaClass primaryMetaClass = DataContext.Current.GetMetaClass(primaryClassName);

            if (relatedMetaClass.Fields[MetaClassManager.GetPrimaryKeyName(primaryClassName)] != null) return;
            using (MetaClassManagerEditScope managerEditScope = DataContext.Current.MetaModel.BeginEdit())
            {
                var metaField = relatedMetaClass.CreateReference(primaryMetaClass, primaryClassName, primaryClassName, true);
                metaField.AccessLevel = AccessLevel.Customization;

                metaField.Attributes.Add(Constants.Attributes.DisplayBlock, Constants.SectionName);
                metaField.Attributes.Add(Constants.Attributes.DisplayText, relatedClassName);
                metaField.Attributes.Add(Constants.Attributes.DisplayOrder, Constants.DefaultDisplayOrder);
                XmlBuilder.ClearCache();

                managerEditScope.SaveChanges();
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }
    }

}