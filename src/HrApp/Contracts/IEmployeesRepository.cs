using HrApp.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IEmployeesRepository
    {
        Task<List<EmployeeEntity>> GetEmployees(Division division);

        Task UpdateEmployeeTimeWorked(string employeeId, double time);
        Task<EmployeeEntity> GetEmployeeByLastName(string lastname);
        Task<EmployeeEntity> GetEmployeeById(string employeeId);
    }
}