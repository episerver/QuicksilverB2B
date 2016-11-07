namespace EPiServer.Reference.Commerce.Site.B2B
{
    public static class Constants
    {
        public static class Classes
        {
            public static string Budget = "Budget";
            public static string BudgetFriendly = "Budget";
            public static string Organization = "Organization";
            public static string Contact = "Contact";
        }
        public static class Fields
        {
            public static string StartDate = "StartDate";
            public static string StartDateFriendly = "Start Date";
            public static string DueDate = "DueDate";
            public static string DueDateFriendly = "Due Date";
            public static string Amount = "Amount";
        }

        public static class Forms
        {
            public const string EditForm = "[MC_BaseForm]";
            public const string ShortInfoForm = "[MC_ShortViewForm]";
            public const string ViewForm = "[MC_GeneralViewForm]";
        }

        public static class Attributes
        {
            public static string DisplayBlock = "Ref_DisplayBlock";
            public static string DisplayText = "Ref_DisplayText";
            public static string DisplayOrder = "Ref_DisplayOrder";
        }

        public static string SectionName = "InfoBlock";
        public static string DefaultDisplayOrder = "10000";
    }
}
