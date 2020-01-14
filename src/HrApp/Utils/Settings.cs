using System;

namespace HrApp
{
    // TODO: later replace it with CodeMashSettings, which reads from config file.
    public static class Settings
    {
        public static Guid ProjectId { get; set; } = Guid.Parse("4a988807-b77f-4518-8e4a-d59eaa4da592");
        public static string ApiKey { get; set; } = "5ad1qmk9ehm-LmieTRmAObODr4DdVNhi";
        
        public static string ReminderAboutFoodTemplateId { get; set; } = "e1671c7f-cc6b-4c20-ba5f-2672a3449c56";
        public static string FoodArrivedTemplateId { get; set; } = "b09eaa56-75eb-42f6-9d77-145ac6f6dedb";
    }
}

