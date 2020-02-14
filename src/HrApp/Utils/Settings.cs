using System;
using CodeMash.Client;

namespace HrApp
{
    // TODO: later replace it with CodeMashSettings, which reads from config file.
    public static class Settings
    {
        public static Guid ProjectId { get; set; } = Guid.Parse("b09eaa56-75eb-42f6-9d77-145ac6f6dedb");
        public static string ApiKey { get; set; } = Environment.GetEnvironmentVariable("ApiKey");
        public static CodeMashClient Client { get; set; } = new CodeMashClient("96WLxsvp7FNolruRNIMYycgVT7rI4_Et", ProjectId);
        public static string ReminderAboutFoodTemplateId { get; set; } = "5e79f236-a85e-4833-8f98-581e626895bf";
        public static string FoodArrivedTemplateId { get; set; } = "35858c5a-45ce-4a86-826a-4aca77c35d8f";
    }
}