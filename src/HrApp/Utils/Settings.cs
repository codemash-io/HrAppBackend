using System;

namespace HrApp
{
    // TODO: later replace it with CodeMashSettings, which reads from config file.
    public static class Settings
    {
        public static Guid ProjectId { get; set; } = Guid.Parse("f5e102ab-db85-41ad-8f7e-c4d414948ce6");
        public static string ApiKey { get; set; } = "5bKsae6eB6TVwoz50ktFN5WT7JPOmsIy";
        
        public static string ReminderAboutFoodTemplateId { get; set; } = "b09eaa56-75eb-42f6-9d77-145ac6f6dedb";
        public static string FoodArrivedTemplateId { get; set; } = "b09eaa56-75eb-42f6-9d77-145ac6f6dedb";

        //all settings for ms office365 calendar
        public static string ClientId = "3d289c6d-1a82-4310-9b39-3b086bc911d9";
        public static string AppSecret = "GCijr[UB]p9cbW78Zs0YsA]c=-KE5j9X";
        public static string TenantId = "a4a29742-209d-4f5f-abdd-395b9defc82a";
    
    }
}

