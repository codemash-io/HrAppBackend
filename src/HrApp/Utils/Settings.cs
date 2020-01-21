using System;
using CodeMash.Client;

namespace HrApp
{
    // TODO: later replace it with CodeMashSettings, which reads from config file.
    public static class Settings
    {
        public static Guid ProjectId { get; set; } = Guid.Parse("b09eaa56-75eb-42f6-9d77-145ac6f6dedb");
        public static string ApiKey { get; set; } = "PDUYEtcEIYBf64IquYeRqnp1WVsNAN5S";
        public static CodeMashClient Client = new CodeMashClient(ApiKey, ProjectId);
        public static string ReminderAboutFoodTemplateId { get; set; } = "b09eaa56-75eb-42f6-9d77-145ac6f6dedb";
        public static string FoodArrivedTemplateId { get; set; } = "b09eaa56-75eb-42f6-9d77-145ac6f6dedb";
    }
}

