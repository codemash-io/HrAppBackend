using System.Collections.Generic;
using System.Threading.Tasks;
using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HrApp
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task<List<EmployeeEntity>> GetEmployees(Division division)
        {
            var repo = new CodeMashRepository<EmployeeEntity>(Client);
            var filter = Builders<EmployeeEntity>.Filter.Eq("division", BsonObjectId.Create(division.Id));
            var employees = await repo.FindAsync(filter, new DatabaseFindOptions()
            {
                PageNumber = 0,
                PageSize = 100
            });
            return employees.Result;
        }
    }
}