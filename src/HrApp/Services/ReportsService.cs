using HrApp.Contracts;
using HrApp.Domain;
using HrApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp.Services
{
    public class ReportsService : IReportsService
    {
        public IProjectRepository projectRepository { get; set; }
        public bool CanEmployeeAccessReports(EmployeeEntity employee)
        {
            if (employee.Role == 2)
                return true;

            return false;
        }

        public List<ProjectEntity> SortProjects(DateTime from, DateTime to, List<ProjectEntity> projects)
        {
            List<ProjectEntity> sortedProjects = new List<ProjectEntity>();
            sortedProjects = projectRepository.SortProjects(from, to).Result;

          /*  foreach (ProjectEntity project in projects)
            {
                if (project.DateCreated >= from && project.DateCreated <= to)
                {
                    sortedProjects.Add(project);
                }
            }*/


            return sortedProjects;
        }
    }
}
