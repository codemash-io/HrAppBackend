using HrApp.Entities;
using System;
using System.Collections.Generic;

namespace HrApp.Contracts
{
    public interface IReportsService
    {
       List<ProjectEntity> SortProjects(DateTime from, DateTime to, List<ProjectEntity> projects);
       bool CanEmployeeAccessReports(EmployeeEntity employee);

        /*List<ProjectEntity> SortProjects(DateTime dateTo, List<ProjectEntity> projects);
        List<ProjectEntity> SortProjects(List<ProjectEntity> projects, DateTime dateFrom);
        List<ProjectEntity> SortProjects(EmployeeEntity employee, List<ProjectEntity> projects);
        List<ProjectEntity> SortProjects(ProjectEntity project, List<ProjectEntity> projects);
        ProjectEntity GetProjectDetails(ProjectEntity project, EmployeeEntity employee);*/

    }
}
