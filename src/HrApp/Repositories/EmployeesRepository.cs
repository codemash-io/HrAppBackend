using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeMash.Client;

namespace HrApp
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public Task<List<EmployeeEntity>> GetEmployees(Division division)
        {
            throw new NotImplementedException();
        }
    }
}