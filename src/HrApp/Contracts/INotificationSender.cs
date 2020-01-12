using System;
using System.Collections.Generic;

namespace HrApp
{
    public interface INotificationSender
    {

        public void SendReminderAboutFoodOrder(List<Guid> receivers);

        public void SendNotificationAboutFoodIsArrived(List<Guid> receivers);
    }
}
