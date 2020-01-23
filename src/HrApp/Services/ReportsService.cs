using HrApp.Contracts;
using HrApp.Entities;
using System;
using System.Collections.Generic;

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

        public List<ProjectEntity> SortProjects(DateTime from, DateTime to)
        {
            List<ProjectEntity> sortedProjects = new List<ProjectEntity>();
            sortedProjects = projectRepository.SortProjects(from, to).Result;
            return sortedProjects;
        }

        public List<ProjectEntity> SortProjectsFrom(DateTime from)
        {
            List<ProjectEntity> sortedProjects = new List<ProjectEntity>();
            sortedProjects = projectRepository.SortProjectsFrom(from).Result;
            return sortedProjects;
        }

        public List<ProjectEntity> SortProjectsTo(DateTime to)
        {
            List<ProjectEntity> sortedProjects = new List<ProjectEntity>();
            sortedProjects = projectRepository.SortProjectsTo(to).Result;
            return sortedProjects;
        }

        public ProjectEntity GetProjectDetails(string projectName)
        {
            var project = projectRepository.GetProjectByName(projectName).Result;
            return project;
        }
    }
}
