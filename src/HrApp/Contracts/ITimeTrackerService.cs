using HrApp.Domain;
using HrApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp.Contracts
{
    public interface ITimeTrackerService
    {
        Task LogHours(EmployeeEntity employee, ProjectEntity project, TimeSpan time, string description);
        Task LogHours(List<ProjectEntity> projects, List<Commit> commits);
        bool CheckIfEmployeeCanWorkOnTheProject(EmployeeEntity employee, ProjectEntity project);
        bool CheckIfEmployeeWorkedMoreThanPossible(List<Commit> commits);
        bool CheckIfProjectBudgetExceeded(Project project);
        Task<bool> CheckForEmployeeOvertime(EmployeeEntity employee);
    }
}
