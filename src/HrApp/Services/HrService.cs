using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public class HrService : IHrService
    {
        public IEmployeesRepository EmployeesRepository { get; set; }
        public async Task<List<EmployeeEntity>> GetEmployeesWhoWorksOnLunchDay(Division division, DateTime lunchDate)
        {
           

            var employees = await EmployeesRepository.GetEmployeesWhoAreNotInBussinessTrip(division, lunchDate);
        
            return await EmployeesRepository.GetEmployeesWhoNotAbsence(employees, lunchDate);
         
        }

        public bool EmployeesAreNotAttending(Employee[] employees)
        {
            throw new NotImplementedException();
        }
    }
}
