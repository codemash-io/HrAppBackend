using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IHrService
    {
        public Task<List<EmployeeEntity>> GetEmployeesWhoWorksOnLunchDay(Division division, DateTime lunchDate);
        
        /// <summary>
        /// Checks if booking employees are not attending another meeting at the same time
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        bool EmployeesAreNotAttending(Employee[] employees);
    }
}
