using CodeMash.Client;
using CodeMash.Repository;
using HrApp.Contracts;
using HrApp.Domain;
using HrApp.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task AddCommtToProject(string commitId, string projectId)
        {
            var repo = new CodeMashRepository<ProjectEntity>(Client);
            await repo.UpdateOneAsync(x => x.Id == projectId,
            Builders<ProjectEntity>.Update.AddToSet($"commits.items", commitId), null);
        }

        public async Task AddEmployeeToProject(Employee employee, Project project)
        {
            var repo = new CodeMashRepository<ProjectEntity>(Client);
            await repo.UpdateOneAsync(x => x.Id == project.Id,
            Builders<ProjectEntity>.Update.AddToSet($"employees.items[{employee}]", employee.Id), null);
        }

        public async Task<string> InsertProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Cannot insert project in db, because project is empty");
            }

            var repo = new CodeMashRepository<ProjectEntity>(Client);

            var entity = new ProjectEntity
            {
                Name = project.Name,
                Description = project.Description,
                Budget = project.Budget,
                Employees = project.Employees.Select(x => x.Id).ToList(),
                DateCreated = project.DateCreated,
                Commits = project.Commits.Select(x => x.Id).ToList()
            };

            var response = await repo.InsertOneAsync(entity, new DatabaseInsertOneOptions());
            return response.Result.Id;
        }


        public async Task<List<ProjectEntity>> SortProjects(DateTime from, DateTime to)
        {
            var repo = new CodeMashRepository<ProjectEntity>(Client);

            var filter = Builders<ProjectEntity>.Filter.Eq("date_created", BsonDateTime.Create(new DateTime(2020,01,14)));
            var sortedProjeccts = await repo.FindAsync(filter, new DatabaseFindOptions()
            {
                PageNumber = 0,
                PageSize = 100
            });
            return sortedProjeccts.Result;
        }


    }
}
