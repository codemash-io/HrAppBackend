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

        public async Task<List<EmployeeEntity>> GetEmployeesWhoAreNotInBussinessTrip(Division division, DateTime lunchDate)
        {
            var repo = new CodeMashRepository<EmployeeEntity>(Client);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            lunchDate = lunchDate.AddHours(4);
            var lunchTime = (DateTime.SpecifyKind(lunchDate, DateTimeKind.Local).ToUniversalTime() - epoch).TotalMilliseconds;
         
            FilterDefinition<EmployeeEntity>[] filters =
           {
                Builders<EmployeeEntity>.Filter.Eq("division", BsonObjectId.Create(division.Id)),
                !(Builders<EmployeeEntity>.Filter.Gte("business_trips.to", lunchTime) & Builders<EmployeeEntity>.Filter.Lte("business_trips.from", lunchTime))               
            };

            var filter = Builders<EmployeeEntity>.Filter.And(filters);              
            var employeesNotInBussinessTrip = await repo.FindAsync(filter, new DatabaseFindOptions());        

            return employeesNotInBussinessTrip.Items;
        }
        public async Task<List<EmployeeEntity>> GetEmployeesWhoNotAbsence(List<EmployeeEntity> employees, DateTime lunchDate)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            lunchDate = lunchDate.AddHours(4);
            var lunchTime = (DateTime.SpecifyKind(lunchDate, DateTimeKind.Local).ToUniversalTime() - epoch).TotalMilliseconds;

            var repo = new CodeMashRepository<AbsenceRequestEntity>(Client);

            FilterDefinition<AbsenceRequestEntity>[] filters =
            {
                Builders<AbsenceRequestEntity>.Filter.Gte("end", lunchTime),
                Builders<AbsenceRequestEntity>.Filter.Lte("start", lunchTime),
                Builders<AbsenceRequestEntity>.Filter.Eq("status", AbsenceRequestStatus.Approved.ToString())
            };
            var filter = Builders<AbsenceRequestEntity>.Filter.And(filters);
            var absenceList = await repo.FindAsync(filter, new DatabaseFindOptions());
           
            foreach (var absence in absenceList.Items)
            {
                employees.RemoveAll(r => r.Id == absence.Employee);
            }

            return employees;
        }

        public async Task UpdateEmployeeTimeWorked(string employeeId, double time)
        {
            var repo = new CodeMashRepository<EmployeeEntity>(Client);

            await repo.UpdateOneAsync(x => x.Id == employeeId,
                Builders<EmployeeEntity>.Update.Set(x => x.TimeWorked, Math.Round(time, 2)), null);
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