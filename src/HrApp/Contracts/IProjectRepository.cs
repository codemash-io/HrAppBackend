using HrApp.Domain;
using HrApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp.Contracts
{
    public interface IProjectRepository
    {
        Task<string> InsertProject(Project project);
        Task AddCommtToProject(string commitId, string projectId);
        Task AddEmployeeToProject(Employee employee, Project project);
        Task<List<ProjectEntity>> SortProjects(DateTime from, DateTime to);
    }
}
