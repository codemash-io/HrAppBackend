using CodeMash.Client;
using CodeMash.Repository;
using HrApp.Contracts;
using HrApp.Domain;
using HrApp.Entities;
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

        public async Task AddCommtToProject(Commit commit, Project project)
        {
            var repo = new CodeMashRepository<ProjectEntity>(Client);
            await repo.UpdateOneAsync(x => x.Id == project.Id,
            Builders<ProjectEntity>.Update.AddToSet($"commits.items[{commit}]", commit.Id), null);
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


    }
}
