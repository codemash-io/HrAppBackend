using CodeMash.Client;
using CodeMash.Repository;
using HrApp.Contracts;
using HrApp.Domain;
using HrApp.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp.Repositories
{
    public class CommitRepository : ICommitRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task<List<CommitEntity>> GetCommitsByEmployee(EmployeeEntity employee)
        {
            var repo = new CodeMashRepository<CommitEntity>(Client);
            var filter = Builders<CommitEntity>.Filter.Eq("employee", BsonObjectId.Create(employee.Id));
            var commits = await repo.FindAsync(filter, new DatabaseFindOptions()
            {
                PageNumber = 0,
                PageSize = 100
            });
            return commits.Items;
        }

        public async Task<CommitEntity> InsertCommit(Commit commit)
        {
            if (commit == null)
            {
                throw new ArgumentNullException(nameof(commit), "Cannot insert commit in db, because commit is empty");
            }

            var repo = new CodeMashRepository<CommitEntity>(Client);

            var entity = new CommitEntity
            {
                Description = commit.Description,
                TimeWorked = Math.Round(commit.TimeWorked, 2),
                CommitDate = commit.CommitDate,
                Employee = commit.Employee.Id
            };

            var response = await repo.InsertOneAsync(entity, new DatabaseInsertOneOptions());
            return response;
        }

        public async Task AddEmployeeToCommit(string commitId, string employeeId)
        {
            var repo = new CodeMashRepository<CommitEntity>(Client);

            var a = await repo.UpdateOneAsync(x => x.Id == commitId,
                            Builders<CommitEntity>.Update.Set(x => x.Employee, employeeId), null);

        }

        public async Task<CommitEntity> GetCommitByDesc(string desc)
        {
            var repo = new CodeMashRepository<CommitEntity>(Client);
            var filter = Builders<CommitEntity>.Filter.Eq("description", BsonString.Create(desc));
            var commit = await repo.FindOneAsync(filter, new DatabaseFindOneOptions());

            return commit;
        }


        public async Task<CommitEntity> GetCommitById(string commitId)
        {
            var repo = new CodeMashRepository<CommitEntity>(Client);
            var commit = await repo.FindOneAsync(x => x.Id == commitId, new DatabaseFindOneOptions());

            return commit;
        }
    }
}
