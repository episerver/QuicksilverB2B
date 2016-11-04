using System.Linq;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
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
            //var customerMetadata =
            //    DataContext.Current.MetaModel.MetaClasses.Cast<MetaClass>()
            //        .First(mc => mc.Name == metaClassName);
            //CreateMetaField(customerMetadata, metaFieldName, MetaFieldType.Integer);
        }

        private void CreateMetaField(MetaClass metaClass, string metaFieldName, string type)
        {
            if (metaClass.Fields[metaFieldName] == null)
            {
                metaClass.CreateMetaField(metaFieldName, metaFieldName, type, true, "", new AttributeCollection());
                FormController.AddMetaPrimitive(metaClass.Name, Forms.EditForm, metaFieldName);
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }
    }

    public class Forms
    {
        public const string EditForm = "[MC_BaseForm]";
        public const string ShortInfoForm = "[MC_ShortViewForm]";
        public const string ViewForm = "[MC_GeneralViewForm]";
    }
}