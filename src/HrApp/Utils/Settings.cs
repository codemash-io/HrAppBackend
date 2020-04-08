using System;
using CodeMash.Client;

namespace HrApp
{
    // TODO: later replace it with CodeMashSettings, which reads from config file.
    public static class Settings
    {
        public static Guid ProjectId { get; set; } = Guid.Parse("f5e102ab-db85-41ad-8f7e-c4d414948ce6");
        //public static string ApiKey { get; set; } = Environment.GetEnvironmentVariable("ApiKey");
        public static string ApiKey { get; set; } = "IwcJhfM4Igc8PpH0ke-E7k6NFzodnJLq";
        public static string ReminderAboutFoodTemplateId { get; set; } = "5e79f236-a85e-4833-8f98-581e626895bf";
        public static string FoodArrivedTemplateId { get; set; } = "35858c5a-45ce-4a86-826a-4aca77c35d8f";
        public static CodeMashClient Client { get; set; } = new CodeMashClient(ApiKey, ProjectId);
        public static string DateChangedTemplateId { get; set; }//was added

        public static string AbsenceRequestNotificationToManager { get; set; } = "5d393081-66b2-47ac-a1e1-18c17f9d364a";
        public static string AbsenceRequestEmailToManager { get; set; } = "bfc9be45-be67-4a4a-9649-f966d54f7cdb";

        //all settings for ms office365 calendar
        public static string ClientId = "3d289c6d-1a82-4310-9b39-3b086bc911d9";
        public static string AppSecret = "GCijr[UB]p9cbW78Zs0YsA]c=-KE5j9X";
        public static string TenantId = "a4a29742-209d-4f5f-abdd-395b9defc82a";
    }
}