using System;
using System.Collections.Generic;

namespace HrApp
{
    public interface INotificationSender
    {

        void SendAsyncNotificationsToDecide(List<Guid> receivers);

        void SendNotificationToDecide(List<Guid> receivers);
    }
}
