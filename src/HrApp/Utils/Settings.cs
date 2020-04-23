using System;
using CodeMash.Client;

namespace HrApp
{
    // TODO: later replace it with CodeMashSettings, which reads from config file.
    public static class Settings
    {
        public static Guid ProjectId { get; set; } = Guid.Parse("b09eaa56-75eb-42f6-9d77-145ac6f6dedb");
        //Guid.Parse("f5e102ab-db85-41ad-8f7e-c4d414948ce6");
        //public static string ApiKey { get; set; } = Environment.GetEnvironmentVariable("ApiKey");
        public static string ApiKey { get; set; } = "96WLxsvp7FNolruRNIMYycgVT7rI4_Et";//"IwcJhfM4Igc8PpH0ke-E7k6NFzodnJLq";
        public static string ReminderAboutFoodTemplateId { get; set; } = "5e79f236-a85e-4833-8f98-581e626895bf";
        public static string FoodArrivedTemplateId { get; set; } = "35858c5a-45ce-4a86-826a-4aca77c35d8f";
        public static string LunchOrderReportTemplate { get; set; } = "cd315848-9c38-4e41-8d2b-278d194064b6";
        public static string LunchOrderEmployeesReportTemplate { get; set; } = "9aaaf5af-aaec-4ed3-9b9b-7b5054bb145a";
        public static CodeMashClient Client { get; set; } = new CodeMashClient(ApiKey, ProjectId);
        public static string DateChangedTemplateId { get; set; }//was added

        public static string AbsenceRequestNotificationToManager { get; set; } = "5d393081-66b2-47ac-a1e1-18c17f9d364a";
        public static string AbsenceRequestEmailToManager { get; set; } = "bfc9be45-be67-4a4a-9649-f966d54f7cdb";

        //all settings for ms office365 calendar
        public static string ClientId = "3d289c6d-1a82-4310-9b39-3b086bc911d9";
        public static string AppSecret = "GCijr[UB]p9cbW78Zs0YsA]c=-KE5j9X";
        public static string TenantId = "a4a29742-209d-4f5f-abdd-395b9defc82a";

        //ESign
        public static string accessTokenESign { get; set; } = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IjY4MTg1ZmYxLTRlNTEtNGNlOS1hZjFjLTY4OTgxMjIwMzMxNyJ9.eyJUb2tlblR5cGUiOjUsIklzc3VlSW5zdGFudCI6MTU4NzQ1NjU1MiwiZXhwIjoxNTg3NDg1MzUyLCJVc2VySWQiOiJhZmMwNTExNy04Y2RjLTQ3OWYtODNmZS05ZDNkYjRhM2RiNGUiLCJzaXRlaWQiOjEsInNjcCI6WyJzaWduYXR1cmUiLCJjbGljay5tYW5hZ2UiLCJvcmdhbml6YXRpb25fcmVhZCIsInJvb21fZm9ybXMiLCJncm91cF9yZWFkIiwicGVybWlzc2lvbl9yZWFkIiwidXNlcl9yZWFkIiwidXNlcl93cml0ZSIsImFjY291bnRfcmVhZCIsImRvbWFpbl9yZWFkIiwiaWRlbnRpdHlfcHJvdmlkZXJfcmVhZCIsImR0ci5yb29tcy5yZWFkIiwiZHRyLnJvb21zLndyaXRlIiwiZHRyLmRvY3VtZW50cy5yZWFkIiwiZHRyLmRvY3VtZW50cy53cml0ZSIsImR0ci5wcm9maWxlLnJlYWQiLCJkdHIucHJvZmlsZS53cml0ZSIsImR0ci5jb21wYW55LnJlYWQiLCJkdHIuY29tcGFueS53cml0ZSJdLCJhdWQiOiJmMGYyN2YwZS04NTdkLTRhNzEtYTRkYS0zMmNlY2FlM2E5NzgiLCJhenAiOiJmMGYyN2YwZS04NTdkLTRhNzEtYTRkYS0zMmNlY2FlM2E5NzgiLCJpc3MiOiJodHRwczovL2FjY291bnQtZC5kb2N1c2lnbi5jb20vIiwic3ViIjoiYWZjMDUxMTctOGNkYy00NzlmLTgzZmUtOWQzZGI0YTNkYjRlIiwiYW1yIjpbImludGVyYWN0aXZlIl0sImF1dGhfdGltZSI6MTU4NzQ1NjU0OSwicHdpZCI6Ijc4OGRkMjAzLTFhMTktNDRmNy04NmQ0LTMzNmE0MDViMmJhMiJ9.i0qfS6NiU4abSEQvoHKPdRZHgH_kTeq5uT0mSPDgvM4aPnAD-B614_0SasRAs_SF-OvGdZBv-PcsxXcbUEh38tFgqcgrkuPZRzjn9TXZKHEFyT1224klJC4q2eq2oTIarcX9tlYTszMpHvfLSCMYR3PwM9VgZdv_au2rXlLByDbmMZE3vLBuin42xuMx9p6S52wthhpMV2VZTSmfuvFXNL4eJz5GBh1gIc9ej92qGurHw_9zIDhNcPOMlAuPBgXFs-cPHua0ie0FlmRc85jUL5-E5gx5eiXdhud04u5LPY_IJW0yanOS8te0zw1LUQW7kYRwz3JntyzLH9cIWye1Ug";
        // public static string accountIdESign { get; set; } = "10368385";
        public static string accountIdESign { get; set; } = "3e126cce-1a34-44fe-b87a-534cd94db250";

       
    }
}