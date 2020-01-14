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

        public async Task<List<CommitEntity>> GetCommits(Employee employee)
        {
            var repo = new CodeMashRepository<CommitEntity>(Client);
            var filter = Builders<CommitEntity>.Filter.Eq("employee", BsonObjectId.Create(employee.Id));
            var commits = await repo.FindAsync(filter, new DatabaseFindOptions()
            {
                PageNumber = 0,
                PageSize = 100
            });
            return commits.Result;
        }

        public async Task<string> InsertCommit(Commit commit)
        {
            if (commit == null)
            {
                throw new ArgumentNullException(nameof(commit), "Cannot insert commit in db, because commit is empty");
            }

            var repo = new CodeMashRepository<CommitEntity>(Client);

            var entity = new CommitEntity
            {
                Description = commit.Description,
                TimeWorked = commit.TimeWorked,
                CommitDate = commit.CommitDate,
                Employee = commit.Employee.Id
            };

            var response = await repo.InsertOneAsync(entity, new DatabaseInsertOneOptions());
            return response.Result.Id;
        }

    }
}
