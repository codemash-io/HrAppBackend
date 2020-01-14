using System.Collections.Generic;
using System.Threading.Tasks;
using CodeMash.Client;
using CodeMash.Repository;
using HrApp.Domain;
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

        public async Task UpdateEmployeeTimeWorked(string employeeId, double time)
        {
            var repo = new CodeMashRepository<EmployeeEntity>(Client);

            await repo.UpdateOneAsync(x => x.Id == employeeId,
                Builders<EmployeeEntity>.Update.Set(x => x.TimeWorked, time), null);
        }
    }
}