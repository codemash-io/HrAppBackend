using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IEmployeesRepository
    {
        Task<List<EmployeeEntity>> GetEmployees(Division division);
        Task<EmployeeEntity> GetEmployeeById(string id);
    }
}