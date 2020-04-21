using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public class HrService : IHrService
    {
        public IEmployeesRepository EmployeesRepository { get; set; }
        public INotificationSender NotificationSender { get; set; }
        public IAbsenceRepository AbsenceRequestRepository { get; set; }
        public INoobRepository NoobRepository { get; set; }
        public IFileRepository FileRepository { get; set; }
        public async Task<List<EmployeeEntity>> GetEmployeesWhoWorksOnLunchDay(Division division, DateTime lunchDate)
        {          
            var employees = await EmployeesRepository.GetEmployeesWhoAreNotInBussinessTrip(division, lunchDate);
        
            return await EmployeesRepository.GetEmployeesWhoNotAbsence(employees, lunchDate);         
        }


        public async Task SendNotificationToManagerAboutAbsence(string employeeId, string absenceId)
        {
            var employee = await EmployeesRepository.GetEmployeeById(employeeId);
            if (employee.ManagerId != null)
            {
                var manager = await EmployeesRepository.GetEmployeeById(employee.ManagerId);
                var receivers = new List<Guid>() { manager.UserId };
                var absence = await AbsenceRequestRepository.GetAbsenceById(absenceId);
                await  NotificationSender.SendNotificationToManagerAboutEmployeeAbsence(receivers, employeeId, absence);
            }
            else
            {
                throw new BusinessException("Employe doesn't have a manager");
            }

        }

        public async Task SendEmailToManagerAboutAbsence(string employeeId, string absenceId)
        {
            var employee = await EmployeesRepository.GetEmployeeById(employeeId);
            if (employee.ManagerId != null)
            {
                
                var manager = await EmployeesRepository.GetEmployeeById(employee.ManagerId);
                if (manager == null)
                {
                    throw new BusinessException("Employe doesn't have a manager");
                }
                var absence = await AbsenceRequestRepository.GetAbsenceById(absenceId);
                var reason = await AbsenceRequestRepository.GetAbsenceByIdWithNames(absence.Type);
                 await NotificationSender.SendEmailToManagerAboutEmployeeAbsence(manager.BusinessEmail, employee, absence, reason);

            }
            else
            {
                throw new BusinessException("Employe doesn't have a manager");
            }
        }

        public async Task<NoobFormEntity> ProcessNoobForm(string noobFormId)
        {
            var form = await NoobRepository.GetNoobFormById(noobFormId);

            await EmployeesRepository.UpdateInfoFromNoobForm(form);
            if (form.Photo.Count != 0)
            {
                var fileId = FileRepository.GetPhotoId(form.Photo[0]);                              

                var photo = await FileRepository.GetFileBytes(fileId);
               var employee =await EmployeesRepository.GetEmployeeProjectionById(form.Meta.ResponsibleUser);
                var fileName = employee.FirstName + " " + employee.LastName + ".jpg";
                await EmployeesRepository.InsertPhoto(photo, employee.Id, fileName);
            }
            return form;
        }


    }
}
