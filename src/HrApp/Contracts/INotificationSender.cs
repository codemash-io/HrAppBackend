using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public interface INotificationSender
    {

        public Task SendReminderAboutFoodOrder(List<Guid> receivers);

        public Task SendNotificationAboutFoodIsArrived(List<Guid> receivers);
    }
}
