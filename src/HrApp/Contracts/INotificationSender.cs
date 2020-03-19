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
        Task SendNotificationAboutLunchDateChanges(List<Guid> receivers, DateTime lunchTime);
        Task SendNotificationToManagerAboutEmployeeAbsence(List<Guid> manager, string employeeId, AbsenceRequestEntity absence);
        Task SendEmailToManagerAboutEmployeeAbsence(string email, EmployeeEntity employeeId, AbsenceRequestEntity absence, string reason);
    }
}
