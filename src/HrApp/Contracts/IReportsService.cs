using HrApp.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp.Contracts
{
    public interface IReportsService
    {
        List<ProjectEntity> SortProjects(DateTime from, DateTime to);
        List<ProjectEntity> SortProjectsFrom(DateTime from);
        List<ProjectEntity> SortProjectsTo(DateTime to);
        bool CanEmployeeAccessReports(EmployeeEntity employee);
        ProjectEntity GetProjectDetails(string projectName);
    }
}
