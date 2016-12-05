using EPiServer.Commerce.Order;
using Mediachase.Commerce.Orders;

namespace EPiServer.Reference.Commerce.Site.B2B.Extensions
{
    public static class OrderGroupExtensions
    {
        #region OrderGroup extensions
        public static bool IsQuoteCart(this OrderGroup orderGroup)
        {
            return (orderGroup is Cart) && orderGroup.GetParentOrderId() != 0;
        }

        public static int GetParentOrderId(this OrderGroup orderGroup)
        {
            return orderGroup.GetIntegerValue(Constants.Quote.ParentOrderGroupId);
        }

        public static int GetIntegerValue(this OrderGroup orderGroup, string fieldName)
        {
            return orderGroup.GetIntegerValue(fieldName, 0);
        }

        public static int GetIntegerValue(this OrderGroup orderGroup, string fieldName, int defaultValue)
        {
            if (orderGroup[fieldName] == null)
            {
                return defaultValue;
            }
            int retVal;
            return int.TryParse(orderGroup[fieldName].ToString(), out retVal) ? retVal : defaultValue;
        }
        #endregion

        #region ICart extensions
        public static bool IsQuoteCart(this ICart orderGroup)
        {
            return orderGroup.GetParentOrderId() != 0;
        }

        public static int GetParentOrderId(this ICart orderGroup)
        {
            return orderGroup.GetIntegerValue(Constants.Quote.ParentOrderGroupId);
        }

        public static int GetIntegerValue(this ICart orderGroup, string fieldName)
        {
            return orderGroup.GetIntegerValue(fieldName, 0);
        }

        public static int GetIntegerValue(this ICart orderGroup, string fieldName, int defaultValue)
        {
            if (orderGroup.Properties[fieldName] == null)
            {
                return defaultValue;
            }
            int retVal;
            return int.TryParse(orderGroup.Properties[fieldName].ToString(), out retVal) ? retVal : defaultValue;
        } 
        #endregion
    }
}
