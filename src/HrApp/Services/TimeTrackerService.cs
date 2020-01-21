using HrApp.Contracts;
using HrApp.Domain;
using HrApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp.Services
{
    public class TimeTrackerService : ITimeTrackerService
    {
        public ICommitRepository CommitRepository { get; set; }
        public IProjectRepository ProjectRepository { get; set; }
        public IEmployeesRepository EmployeeRepository { get; set; }

        //start - stop method
        public async Task LogHours(EmployeeEntity employee, ProjectEntity project, TimeSpan time, string description)
        {
            if (!CheckIfEmployeeCanWorkOnTheProject(employee, project))
            {
                throw new BusinessException("You cannot work on a project not assigned to you");
            }
            else if(time > TimeSpan.FromHours(16))
            {
                throw new BusinessException("You cannot work more than 16h!");
            }
            else
            {
                //creating commit for a project
                Commit commit = new Commit(employee, description, time.TotalHours);

                //adding commit to db
                var commitId = await CommitRepository.InsertCommit(commit);
                commit.Id = commitId.Id;

                //adding commit to a project
                await ProjectRepository.AddCommitToProject(commit.Id, project.Id);


                //adding time employee worked on current project
                var totalTime = employee.TimeWorked + time.TotalHours;
                await EmployeeRepository.UpdateEmployeeTimeWorked(employee.Id, totalTime);


               /* if (CheckForEmployeeOvertime(employee))
                {
                    //should i send notification here?
                    var message = "Jus dirbote viršvalandžius";
                }*/
            }
        }

        //multiple projects - multiple hours method

        public async Task LogHours(List<ProjectEntity> projects, List<Commit> commits)
        {
            //checking for empty lists
            if (projects == null)
            {
                throw new ArgumentNullException(nameof(projects), "No projects defined");
            }
            else if (commits == null)
            {
                throw new ArgumentNullException(nameof(commits), "No commits defined");
            }

            if (projects.Count != commits.Count)
            {
                throw new BusinessException("Projects and commits count do not match!");
            }
            for (int i = 0; i < projects.Count; i++)
            {
                if (!CheckIfEmployeeCanWorkOnTheProject(commits[i].Employee, projects[i]))
                {
                    throw new BusinessException("You cannot work on a project not assigned to you");
                }

                else if (CheckIfEmployeeWorkedMoreThanPossible(commits))
                {
                    throw new BusinessException("You cannot work more than 16h during a day!");
                }
                else
                {
                    //adding commits to db
                    var commit = await CommitRepository.InsertCommit(commits[i]);
                    commits[i].Id = commit.Id;


                    //adding time employee worked on current project
                    var totalTime = commits[i].Employee.TimeWorked + commits[i].TimeWorked;
                    await EmployeeRepository.UpdateEmployeeTimeWorked(commits[i].Employee.Id, totalTime);

                    //adding commits to a projects
                    await ProjectRepository.AddCommitToProject(commits[i].Id, projects[i].Id);

                }
            }
        }

        public bool CheckIfEmployeeCanWorkOnTheProject(EmployeeEntity employee, ProjectEntity project)
        {
            foreach (var emp_id in project.Employees)
            {
                if (emp_id == employee.Id)
                {
                    return true;
                }
            }

            return false;
        }
        public bool CheckIfEmployeeCanWorkOnTheProject(string employeeId, ProjectEntity project)
        {
            foreach (var emp_id in project.Employees)
            {
                if (emp_id == employeeId)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CheckIfEmployeeWorkedMoreThanPossible(List<CommitEntity> commits)
        {
            TimeSpan totalTime = TimeSpan.FromHours(CalculateCommitsTime(commits));

            if (totalTime > TimeSpan.FromHours(16))
            {
                return true;
            }

            return false;
        }

        public bool CheckIfEmployeeWorkedMoreThanPossible(List<Commit> commits)
        {
            TimeSpan totalTime = TimeSpan.FromHours(CalculateCommitsTime(commits));

            if (totalTime > TimeSpan.FromHours(16))
            {
                return true;
            }

            return false;
        }

        public bool CheckIfProjectBudgetExceeded(Project project)
        {
            //calculating total time of all project commits
            var totalTime = CalculateCommitsTime(project.Commits);

            //project budget exceeded or equals to zero
            if (project.Budget <= totalTime)
                return true;

            return false;
        }

        public bool CheckForEmployeeOvertime(EmployeeEntity employee)
        {
            //getting all commits by employee
            var commits = CommitRepository.GetCommitsByEmployee(employee);
            //calculating total work time of all employee commits
            double totaTtime = CalculateCommitsTime(commits.Result);       

            // if employee exceeded monthly hours 
            if (employee.Budget < totaTtime)
            {
                return true;
            }

            return false;
        }

        private double CalculateCommitsTime(List<CommitEntity> commits)
        {
            double totaTtime = 0;
            foreach (var commit in commits)
            {
                totaTtime += commit.TimeWorked;
            }
            return totaTtime;
        }

        private double CalculateCommitsTime(List<Commit> commits)
        {
            double totaTtime = 0;
            foreach (var commit in commits)
            {
                totaTtime += commit.TimeWorked;
            }
            return totaTtime;
        }
    }
}
