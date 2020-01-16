using System;

namespace HrApp
{
    // TODO: later replace it with CodeMashSettings, which reads from config file.
    public static class Settings
    {
        public static Guid ProjectId { get; set; } = Guid.Parse("4a988807-b77f-4518-8e4a-d59eaa4da592");
        public static string ApiKey { get; set; } = "5ad1qmk9ehm-LmieTRmAObODr4DdVNhi";
        
        public static string ReminderAboutFoodTemplateId { get; set; } = "e1671c7f-cc6b-4c20-ba5f-2672a3449c56";
        public static string FoodArrivedTemplateId { get; set; } = "e1671c7f-cc6b-4c20-ba5f-2672a3449c56";

        public static string WelcomeEmail { get; set; } = "cf22fb31-ff6b-4c0c-9764-a12866fd53cd";
    }
}


