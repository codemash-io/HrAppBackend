using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeMash.Client;
using CodeMash.Notifications.Email.Services;
using CodeMash.Notifications.Push.Services;
using Isidos.CodeMash.ServiceContracts;
using MongoDB.Bson;

namespace HrApp
{
    public class NotificationSender: INotificationSender
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task SendReminderAboutFoodOrder(List<Guid> receivers, DateTime lunchTime)
        {
            var pushService = new CodeMashPushService(Client);
            var response = await pushService.SendPushNotificationAsync(
                new SendPushNotificationRequest
                {
                    TemplateId = Guid.Parse(Settings.ReminderAboutFoodTemplateId),
                    Users = receivers,
                    Postpone = (long)1000 * 60 * 60,
                    Tokens = new Dictionary<string, string>
                    {
                        {"LunchDate", lunchTime.ToShortDateString()}
                    }
                }
            );
        }

        public async Task SendNotificationAboutFoodIsArrived(List<Guid> receivers)
        {
            var pushService = new CodeMashPushService(Client);
            var response = await pushService.SendPushNotificationAsync(
                new SendPushNotificationRequest
                {
                    TemplateId = Guid.Parse(Settings.FoodArrivedTemplateId),
                    Users = receivers
                }
            );
        }
       

        public async Task SendNotificationForNewReceiversAboutLunchDate(List<Guid> receivers, DateTime lunchTime)
        {
            var pushService = new CodeMashPushService(Client);
            var response = await pushService.SendPushNotificationAsync(
                new SendPushNotificationRequest
                {
                    TemplateId = Guid.Parse(Settings.DateChangedTemplateId),
                    Users = receivers,
                    Postpone = (long)1000 * 60 * 60,
                    Tokens = new Dictionary<string, string>
                    {
                        {"LunchDate", lunchTime.ToShortDateString()}
                    }
                }
            );
        }

        public async Task SendNotificationAboutLunchDateChanges(List<Guid> receivers, DateTime lunchTime)
        {
            var pushService = new CodeMashPushService(Client);
            var response = await pushService.SendPushNotificationAsync(
                new SendPushNotificationRequest
                {
                    TemplateId = Guid.Parse(Settings.DateChangedTemplateId),
                    Users = receivers,
                    Postpone = (long)1000 * 60 * 60,
                    Tokens = new Dictionary<string, string>
                    {
                        {"LunchDate", lunchTime.ToShortDateString()}
                    }
                }
            );
        }
        public async Task SendNotificationToManagerAboutEmployeeAbsence(List<Guid> manager,string employeeId,  AbsenceRequestEntity absence)
        {
            var pushService = new CodeMashPushService(Client);
            var response = await pushService.SendPushNotificationAsync(
                new SendPushNotificationRequest
                {
                    TemplateId = Guid.Parse(Settings.AbsenceRequestNotificationToManager),
                    Users = manager,                    
                    Tokens= new Dictionary<string, string>
                    {
                        { "employee",employeeId},
                        { "type", absence.Type},
                        { "From", absence.AbsenceStart.ToString()},
                        { "To", absence.AbsenceEnd.ToString()}
                    },                    
                }
            );
        }
        public async Task SendEmailToManagerAboutEmployeeAbsence(string email, EmployeeEntity employee, AbsenceRequestEntity absence, string reason)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var beginTime = epoch.AddMilliseconds(absence.AbsenceStart).AddHours(8);
            var endTime = epoch.AddMilliseconds(absence.AbsenceEnd).AddHours(8);

            var begin = beginTime.ToUniversalTime().Subtract(
    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    ).TotalMilliseconds;
            var endTimefloat = endTime.ToUniversalTime().Subtract(
    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    ).TotalMilliseconds;

            var start = begin.ToString("0");
            var end = endTimefloat.ToString("0");
            var emailService = new CodeMashEmailService(Client);
            var response = await emailService.SendEmailAsync(
                new SendEmailRequest()
                {
                    TemplateId = Guid.Parse(Settings.AbsenceRequestEmailToManager),
                    Emails = new List<string>() { email },
                    Tokens = new Dictionary<string, string>
                    {
                        { "employee.first_name",employee.FirstName},
                        { "employee.last_name",employee.LastName},
                        { "type.name.en", reason},
                        { "From", start},
                        { "To", end}
                    },                  
                    
                }
            );
        }

        public async Task SendWishlistSummaryEmail( DateTime from, DateTime to, 
                    string email, List<WishlistSummary> summary)
        {
            var begin = from.ToUniversalTime().Subtract( 
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            var endTimefloat = to.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            var start = begin.ToString("0");
            var end = endTimefloat.ToString("0");

            Dictionary<string, string> tokens;
            if (summary.Count == 1)
                tokens = new Dictionary<string, string>
                {
                    { "Total1","- " +summary[0].Total.ToString() + " EUR"},
                    { "Type1", summary[0].Type},
                    { "From", start},
                    { "To", end}
                };
            else if (summary.Count == 2)
                tokens = new Dictionary<string, string>
                {
                    { "Total1","- " +summary[0].Total.ToString() + " EUR"},
                    { "Type1", summary[0].Type},
                    { "Total2","- " +summary[1].Total.ToString() + " EUR"},
                    { "Type2", summary[1].Type},
                    { "From", start},
                    { "To", end}
                };
            else if (summary.Count == 3)
                tokens = new Dictionary<string, string>
                {
                    { "Total1","- " +summary[0].Total.ToString() + " EUR"},
                    { "Type1", summary[0].Type},
                    { "Total2","- " +summary[1].Total.ToString() + " EUR"},
                    { "Type2", summary[1].Type},
                    { "Total3","- " +summary[2].Total.ToString() + " EUR"},
                    { "Type3", summary[2].Type},
                    { "From", start},
                    { "To", end}
                };
            else
                tokens = new Dictionary<string, string>
                {
                    { "Total1","- " +summary[0].Total.ToString() + " EUR"},
                    { "Type1", summary[0].Type},
                    { "Total2","- " +summary[1].Total.ToString() + " EUR"},
                    { "Type2", summary[1].Type},
                    { "Total3","- " +summary[2].Total.ToString() + " EUR"},
                    { "Type3", summary[2].Type},
                    { "Total4","- " +summary[3].Total.ToString() + " EUR"},
                    { "Type4", summary[3].Type},
                    { "From", start},
                    { "To", end}
                };


            var pushService = new CodeMashEmailService(Client);
            await pushService.SendEmailAsync(
                new SendEmailRequest
                {
                    TemplateId = Guid.Parse("3d2a099f-a311-480a-ad61-894a2b8a53f3"),
                    Emails = new List<string>() { email },
                    Tokens = tokens
                }
            );
        }

    }
}
