using Mediachase.BusinessFoundation.Data.Business;

namespace EPiServer.Reference.Commerce.Site.B2B.Extensions
{
    public static class EntityObjectExtensions
    {
        public static string GetStringValue(this EntityObject item, string fieldName)
        {
            return item.GetStringValue(fieldName, string.Empty);
        }

        public static int GetIntegerValue(this EntityObject item, string fieldName)
        {
            return item.GetIntegerValue(fieldName, 0);
        }

        public static string GetStringValue(this EntityObject item, string fieldName, string defaultValue)
        {
            return item[fieldName] != null ? item[fieldName].ToString() : defaultValue;
        }

        public static int GetIntegerValue(this EntityObject item, string fieldName, int defaultValue)
        {
            if (item[fieldName] == null)
            {
                return defaultValue;
            }
            int retVal;
            return int.TryParse(item[fieldName].ToString(), out retVal) ? retVal : defaultValue;
        }

        public static bool GetBoolValue(this EntityObject item, string fieldName)
        {
            return item.GetBoolValue(fieldName, false);
        }

        public static bool GetBoolValue(this EntityObject item, string fieldName, bool defaultValue)
        {
            if (item[fieldName] == null)
            {
                return defaultValue;
            }
            bool retVal;
            return bool.TryParse(item[fieldName].ToString(), out retVal) ? retVal : defaultValue;
        }
    }
}
