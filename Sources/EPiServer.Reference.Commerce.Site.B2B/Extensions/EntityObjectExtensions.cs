using System;
using Mediachase.BusinessFoundation.Data.Business;

namespace EPiServer.Reference.Commerce.Site.B2B.Extensions
{
    public static class EntityObjectExtensions
    {
        public static string GetStringValue(this EntityObject item, string fieldName)
        {
            return item.GetStringValue(fieldName, string.Empty);
        }

        public static string GetStringValue(this EntityObject item, string fieldName, string defaultValue)
        {
            return item[fieldName] != null ? item[fieldName].ToString() : defaultValue;
        }

        public static DateTime GetDateTimeValue(this EntityObject item, string fieldName)
        {
            return item.GetDateTimeValue(fieldName, DateTime.MinValue);
        }

        public static DateTime GetDateTimeValue(this EntityObject item, string fieldName, DateTime defaultValue)
        {
            if (item[fieldName] == null)
            {
                return defaultValue;
            }
            DateTime retVal;
            return DateTime.TryParse(item[fieldName].ToString(), out retVal) ? retVal : defaultValue;
        }

        public static int GetIntegerValue(this EntityObject item, string fieldName)
        {
            return item.GetIntegerValue(fieldName, 0);
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

        public static float GetFloatValue(this EntityObject item, string fieldName)
        {
            return item.GetFloatValue(fieldName, 0);
        }

        public static float GetFloatValue(this EntityObject item, string fieldName, float defaultValue)
        {
            if (item[fieldName] == null)
            {
                return defaultValue;
            }
            float retVal;
            return float.TryParse(item[fieldName].ToString(), out retVal) ? retVal : defaultValue;
        }

        public static decimal GetDecimalValue(this EntityObject item, string fieldName)
        {
            return item.GetDecimalValue(fieldName, 0);
        }

        public static decimal GetDecimalValue(this EntityObject item, string fieldName, decimal defaultValue)
        {
            if (item[fieldName] == null)
            {
                return defaultValue;
            }
            decimal retVal;
            return decimal.TryParse(item[fieldName].ToString(), out retVal) ? retVal : defaultValue;
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

        public static Guid GetGuidValue(this EntityObject item, string fieldName)
        {
            return item.GetGuidValue(fieldName, Guid.Empty);
        }

        public static Guid GetGuidValue(this EntityObject item, string fieldName, Guid defaultValue)
        {
            if (item[fieldName] == null)
            {
                return defaultValue;
            }
            Guid retVal;
            return Guid.TryParse(item[fieldName].ToString(), out retVal) ? retVal : defaultValue;
        }
    }
}
