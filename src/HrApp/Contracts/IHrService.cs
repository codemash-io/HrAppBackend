using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IHrService
    {
        Task<List<EmployeeEntity>> GetEmployeesWhoWorksOnLunchDay(Division division, DateTime lunchDate);

        /// <summary>
        /// Checks if booking employees are not attending another meeting at the same time
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        Task SendNotificationToManagerAboutAbsence(string employeeId, string absenceId);
        Task SendEmailToManagerAboutAbsence(string employeeId, string absenceId);
        Task<NoobFormEntity> ProcessNoobForm(string NoobFormId);
        Task GenerateFileWithSignatureAndInsert(string absenceId);
    }
}
