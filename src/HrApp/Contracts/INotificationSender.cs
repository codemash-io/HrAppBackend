using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public interface INotificationSender
    {

  
        Task SendReminderAboutFoodOrder(List<Guid> receivers, DateTime lunchTime);

        Task SendNotificationAboutFoodIsArrived(List<Guid> receivers);

        Task SendNotificationForNewReceiversAboutLunchDate(List<Guid> receivers, DateTime lunchTime);

    }
}
