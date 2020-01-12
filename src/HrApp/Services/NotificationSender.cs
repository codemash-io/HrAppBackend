using System;
using System.Collections.Generic;
using CodeMash.Client;
using CodeMash.Notifications.Push.Services;
using Isidos.CodeMash.ServiceContracts;

namespace HrApp
{
    public class NotificationSender: INotificationSender
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public void SendReminderAboutFoodOrder(List<Guid> receivers)
        {
            var pushService = new CodeMashPushService(Client);
            var response = pushService.SendPushNotification(
                new SendPushNotificationRequest
                {
                    TemplateId = Guid.Parse(Settings.ReminderAboutFoodTemplateId),
                    Users = receivers
                }
            );
        }

        public void SendNotificationAboutFoodIsArrived(List<Guid> receivers)
        {
            var pushService = new CodeMashPushService(Client);
            var response = pushService.SendPushNotification(
                new SendPushNotificationRequest
                {
                    TemplateId = Guid.Parse(Settings.FoodArrivedTemplateId),
                    Users = receivers
                }
            );
        }
    }
}
