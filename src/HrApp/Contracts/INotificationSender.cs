using System;
using System.Collections.Generic;

namespace HrApp
{
    public interface INotificationSender
    {

        void SendReminderAboutFoodOrder(List<Guid> receivers);

        void SendNotificationAboutFoodIsArrived(List<Guid> receivers);
    }
}
