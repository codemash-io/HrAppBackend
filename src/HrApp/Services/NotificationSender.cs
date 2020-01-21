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
        private static int SyncDecisionMakingCount;

        static NotificationSender()
        {
            SyncDecisionMakingCount = 0;
        }
        public void SendAsyncNotificationsToDecide(List<Guid> receivers)
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

        public void SendNotificationToDecide(List<Guid> receivers)
        {
            var pushService = new CodeMashPushService(Client);
            var response = pushService.SendPushNotification(
                new SendPushNotificationRequest
                {
                    TemplateId = Guid.Parse(Settings.FoodArrivedTemplateId),
                    Users = new List<Guid> { receivers[SyncDecisionMakingCount] }
                }
            );

            SyncDecisionMakingCount++;
        }
    }
}
