using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Mediachase.Commerce.Catalog;
using Mediachase.MetaDataPlus;
using Mediachase.MetaDataPlus.Configurator;
using EPiServer.Reference.Commerce.Site.B2B;

namespace EPiServer.Reference.Commerce.Site.Infrastructure.InitializationModules
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    public class MetaDataPlusInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            MetaDataContext mdContext = CatalogContext.MetaDataContext;
            AddMetaFieldToClass(mdContext, "Mediachase.Commerce.Orders.System", "OrderFormEx", Constants.Quote.QuoteExpireDate, MetaDataType.DateTime, 255,
                true, false);
            AddMetaFieldToClass(mdContext, "Mediachase.Commerce.Orders.System", "OrderFormEx", Constants.Quote.QuoteStatus, MetaDataType.LongString, 255,
                true, false);
            AddMetaFieldToClass(mdContext, "Mediachase.Commerce.Orders.System", "OrderFormEx", Constants.Quote.PreQuoteTotal, MetaDataType.Decimal, 255,
                true, false, 9, 38);
            AddMetaFieldToClass(mdContext, "Mediachase.Commerce.Orders.System", "OrderFormEx", Constants.Customer.CustomerFullName, MetaDataType.LongString, 255,
                true, false, 9, 38);
            AddMetaFieldToClass(mdContext, "Mediachase.Commerce.Orders.System", "OrderFormEx", Constants.Customer.CurrentCustomerOrganization, MetaDataType.LongString, 255,
                true, false, 9, 38);
            AddMetaFieldToClass(mdContext, "Mediachase.Commerce.Orders.System", "OrderFormEx", Constants.Customer.CustomerEmailAddress, MetaDataType.LongString, 255,
                true, false, 9, 38);
            AddMetaFieldToClass(mdContext, "Mediachase.Commerce.Orders.System", "LineItemEx", Constants.Quote.PreQuotePrice, MetaDataType.Decimal, 38,
               true, false, 9, 38);
            AddMetaFieldToClass(mdContext, "Mediachase.Commerce.Orders.System", "ShoppingCart", Constants.Quote.ParentOrderGroupId, MetaDataType.Int, 255,
               true, false);
        }

        private void AddMetaFieldToClass(MetaDataContext mdContext, string metaDataNamespace, string metaClassName,
            string metaFieldName, MetaDataType type, int length, bool allowNulls, bool cultureSpecific, int optScale = 2, int optPrecision = 18)
        {
            var metaField = CreateMetaField(mdContext, metaDataNamespace, metaFieldName, type, length, allowNulls,
                cultureSpecific,optScale, optPrecision);
            JoinField(mdContext, metaField, metaClassName);
        }

        private MetaField CreateMetaField(MetaDataContext mdContext, string metaDataNamespace, string metaFieldName, MetaDataType type, int length, bool allowNulls, bool cultureSpecific, int optScale = 2, int optPrecision = 18 )
        {
            var metaField = MetaField.Load(mdContext, metaFieldName) ??
                    MetaField.Create(mdContext, metaDataNamespace, metaFieldName, metaFieldName, string.Empty, type, length, allowNulls, cultureSpecific, false, false);

            if (type != MetaDataType.Decimal) return metaField;

            metaField.Attributes[MetaFieldAttributeConstants.MdpPrecisionAttributeName] = optPrecision.ToString();
            metaField.Attributes[MetaFieldAttributeConstants.MdpScaleAttributeName] = optScale.ToString();
            return metaField;
        }

        private void JoinField(MetaDataContext mdContext, MetaField field, string metaClassName)
        {
            var cls = MetaClass.Load(mdContext, metaClassName);
            if (!MetaFieldIsNotConnected(field, cls)) return;

            cls.AddField(field);
        }

        private static bool MetaFieldIsNotConnected(MetaField field, MetaClass cls)
        {
            return cls != null && !cls.MetaFields.Contains(field);
        }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }
    }
}