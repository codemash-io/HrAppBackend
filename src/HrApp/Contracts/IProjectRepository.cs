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
        Task AddCommitToProject(string commitId, string projectId);
        Task AddEmployeeToProject(string employeeId, string projectId);
        Task<ProjectEntity> GetProjectById(string projectId);
        Task<ProjectEntity> GetProjectByName(string name);
        //reports
        Task<List<ProjectEntity>> SortProjects(DateTime from, DateTime to);
        Task<List<ProjectEntity>> SortProjectsFrom(DateTime from);
        Task<List<ProjectEntity>> SortProjectsTo(DateTime to);
    }
}
