using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeMash.Client;
using CodeMash.Project.Services;
using CodeMash.Repository;
using HrApp.Domain;
using Isidos.CodeMash.ServiceContracts;
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

           // await repo.UpdateOneAsync(x => x.Id == employeeId,
              //  Builders<EmployeeEntity>.Update.Set(x => x.TimeWorked, Math.Round(time, 2)), null);
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
            var employee = await repo.FindOneByIdAsync(employeeId);

            return employee;
        }
        public async Task<EmployeeNameSurnameEntity> GetEmployeeProjectionById(string employeeUserId)
        {
            var repo = new CodeMashRepository<EmployeeNameSurnameEntity>(Client);
            var filterBuilder = Builders<EmployeeNameSurnameEntity>.Filter;
            var filter = filterBuilder.Eq("application_user", employeeUserId);
/*
            var projection = Builders<EmployeeEntity>.Projection
            .Include(x => x.FirstName)
            .Include(x => x.LastName)
            .Include(x => x.Id);
            */
            var employee = await repo.FindAsync<EmployeeNameSurnameEntity>(filter,null);

            return employee.Items[0];
        }
        public async Task<bool> UpdateVacationBalance(Personal person)
        {
            var repo = new CodeMashRepository<EmployeeEntity>(Client);

            var updateBuilder = Builders<EmployeeEntity>.Update;
            var update = updateBuilder.Set(doc => doc.NumberOfHolidays, person.Left);
            var splitName = person.Employee.Split(" ");
            var filterBuilder = Builders<EmployeeEntity>.Filter;
            var filter = filterBuilder.Eq(x=>x.FirstName, splitName[1])
                & filterBuilder.Eq(x => x.LastName, splitName[0]);

            try
            {
                await repo.UpdateOneAsync(filter,update,new DatabaseUpdateOneOptions());
            }
            catch (Exception)
            {
                Logger logger = Logger.GetLogger();
                logger.Log(person.Employee + " does not exist in the database");
                return true;
            }

            return false;
        }
        public async Task UpdateInfoFromNoobForm(NoobFormEntity noobFormEntity)
        {
            var repo = new CodeMashRepository<EmployeeEntity>(Client);
            EmergencyContact emergency = new EmergencyContact() {
                Name = noobFormEntity.EmergencyContactName,
                Phone = noobFormEntity.EmergencyPhone,
                Position = noobFormEntity.RelationsWithContact
            };
            var filterBuilder = Builders<EmployeeEntity>.Filter;
            var filter = filterBuilder.Eq("application_user", noobFormEntity.Meta.ResponsibleUser);

            var updateBuilder = Builders<EmployeeEntity>.Update;
            var update = updateBuilder.Set(emp => emp.IBAN, noobFormEntity.IBAN)
                .Set(emp => emp.Address, noobFormEntity.Address)
                .Set(emp => emp.ComputerPart, noobFormEntity.ComputerPart)
                .Set(emp => emp.DesribeYourselfIn3Words, noobFormEntity.DesribeYourselfIn3Words)
                .Set(emp => emp.DocumentExpirationDate, noobFormEntity.DocumentExpirationDate)
                .Set("emergency_contact", new List<EmergencyContact>() { emergency })
                .Set(emp => emp.FunnyFact, noobFormEntity.FunnyFact)
                .Set(emp => emp.GoAnywhere, noobFormEntity.GoAnywhere)
                .Set(emp => emp.HiddenTalent, noobFormEntity.HiddenTalent)
                .Set(emp => emp.IBAN, noobFormEntity.IBAN)
                .Set(emp => emp.MovieOrBook, noobFormEntity.MovieOrBook)
                .Set(emp => emp.PassOrIDCardNo, noobFormEntity.PassOrIDCardNo)
                .Set(emp => emp.PersonalId, noobFormEntity.PersonalId)                
                .Set(emp => emp.SpareTime, noobFormEntity.SpareTime);


            await repo.UpdateOneAsync(filter, update, new DatabaseUpdateOneOptions());
        }
        public async Task InsertPhoto(byte[] photo, string recordId, string fileName)
        {
            var filesService = new CodeMashFilesService(Client);


            var response = await filesService.UploadRecordFileAsync(photo, fileName,
                     new UploadRecordFileRequest
                     {
                         UniqueFieldName = "photo",
                         CollectionName = "pc-employees",
                         RecordId = recordId

                     }
                 );


        }

    }
}