using HrApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp.Contracts
{
    public interface IProjectRepository
    {
        Task<string> InsertProject(Project commit);
        Task AddCommtToProject(Commit commit, Project project);
        Task AddEmployeeToProject(Employee employee, Project project);
    }
}
