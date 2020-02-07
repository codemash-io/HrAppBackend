using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeMash.Client;
using CodeMash.Notifications.Push.Services;
using Isidos.CodeMash.ServiceContracts;

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
    }
}
