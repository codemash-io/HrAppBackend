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
        public void LogHours(EmployeeEntity employee, ProjectEntity project, TimeSpan time, string description);
        void LogHours(List<ProjectEntity> projects, List<Commit> commits);
        bool CheckIfEmployeeCanWorkOnTheProject(EmployeeEntity employee, ProjectEntity project);
        bool CheckIfEmployeeCanWorkOnTheProject(string employeeId, ProjectEntity project);
        bool CheckIfEmployeeWorkedMoreThanPossible(List<CommitEntity> commits);
        bool CheckIfEmployeeWorkedMoreThanPossible(List<Commit> commits);
        bool CheckIfProjectBudgetExceeded(ProjectEntity project);

        //bool CheckForEmployeeOvertime(EmployeeEntity employee);
    }
}
