using System;
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
            return employees.Items;
        }

        public async Task UpdateEmployeeTimeWorked(string employeeId, double time)
        {
            var repo = new CodeMashRepository<EmployeeEntity>(Client);

            await repo.UpdateOneAsync(x => x.Id == employeeId,
                Builders<EmployeeEntity>.Update.Set(x => x.TimeWorked, Math.Round(time, 1)), null);
        }

        public async Task<EmployeeEntity> GetEmployeeByLastName(string lastname)
        {
            var repo = new CodeMashRepository<EmployeeEntity>(Client);
            var filter = Builders<EmployeeEntity>.Filter.Eq("last_name", BsonString.Create(lastname));
            var employee = await repo.FindOneAsync(filter, new DatabaseFindOneOptions());

            return employee;
        }

        public async Task<EmployeeEntity> GetEmployeeById(string employeeId)
        {
            var repo = new CodeMashRepository<EmployeeEntity>(Client);
            var employee = await repo.FindOneAsync(x => x.Id == employeeId, new DatabaseFindOneOptions());

            return employee;
        }


    }
}