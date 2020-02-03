using System.Threading.Tasks;
using HrApp;
using System;
using NUnit.Framework;
using HrApp.Domain;
using HrApp.Entities;
using HrApp.Repositories;
using HrApp.Services;
using System.Collections.Generic;

namespace Tests
{
    class TimeTrackerIntegrations
    {
        EmployeesRepository empRepo;
        ProjectRepository proRepo;
        CommitRepository commRepo;
        TimeTrackerService tracker;
        ReportsService reporter;

        [SetUp]
        public void Setup()
        {
            empRepo = new EmployeesRepository();
            commRepo = new CommitRepository();
            proRepo = new ProjectRepository();
            tracker = new TimeTrackerService
            {
                CommitRepository = commRepo,
                ProjectRepository = proRepo,
                EmployeeRepository = empRepo
            };
            reporter = new ReportsService
            {
                projectRepository = proRepo
            };

        }
        private async Task TestCanInsertCommit()
        {
            var employee = empRepo.GetEmployeeByLastName("t");

            Commit commit = new Commit(employee.Result, "testas", 2);

            var res = await commRepo.InsertCommit(commit);

            Assert.AreEqual(employee.Result.Id, res.Employee);
        }
        [Test]
        public async Task TestCanGetCommitsByEmployee()
        {
            var employee = empRepo.GetEmployeeByLastName("t");

            await TestCanInsertCommit(); // should insert one commit for spec user

            var res = await commRepo.GetCommitsByEmployee(employee.Result);

            Assert.AreEqual(1, res.Count);
        }
        [Test]
        public async Task TestCanAddEmployeeToCommit()
        {
            await TestCanInsertCommit();

            var emp = empRepo.GetEmployeeByLastName("t");
            var commit = commRepo.GetCommitByDesc("testas");
       
            await commRepo.AddEmployeeToCommit(commit.Result.Id, emp.Result.Id);

            commit = commRepo.GetCommitById(commit.Result.Id);

            Assert.AreEqual(commit.Result.Employee, emp.Result.Id);
        }
        [Test]
        public async Task TestCanUpdateEmployeeTimeWorked()
        {
            var emp = empRepo.GetEmployeeByLastName("t");

            await empRepo.UpdateEmployeeTimeWorked(emp.Result.Id, 10.6);

            emp = empRepo.GetEmployeeById(emp.Result.Id);

            Assert.AreEqual(emp.Result.TimeWorked, 10.6);
        }
        [Test]
        public async Task TestCanInsertProject()
        {
            var employee = empRepo.GetEmployeeByLastName("t");

            List<EmployeeEntity> list = new List<EmployeeEntity>();
            list.Add(employee.Result);

            Project project = new Project("test", "des", 200, list);

            var response = await proRepo.InsertProject(project);

            Assert.AreEqual(response, proRepo.GetProjectById(response).Result.Id);

        }
        [Test]
        public async Task TestCanAddCommitToProject()
        {
            var commit = commRepo.GetCommitByDesc("description");

            await TestCanInsertProject(); // inserting new proejct
            var project = proRepo.GetProjectByName("test");

            await proRepo.AddCommitToProject(commit.Result.Id, project.Result.Id);

            project = proRepo.GetProjectById(project.Result.Id);

            Assert.AreEqual(1, project.Result.Commits.Count);

        }
        [Test]
        public async Task TestCanAddEmployeeToProject()
        {
            var emp = empRepo.GetEmployeeByLastName("t");

            await TestCanInsertProject(); // inserting new proejct
            var project = proRepo.GetProjectByName("test");

            await proRepo.AddEmployeeToProject(emp.Result.Id, project.Result.Id);

            Assert.AreEqual(1, project.Result.Employees.Count);
        }
        [Test]
        public void TestCanSortProjectsFromTo()
        {      
            var from = new DateTime(2020, 01, 05);
            var to = new DateTime(2020, 01, 22);

            var projects = reporter.SortProjects(from, to);

            Assert.AreEqual(3, projects.Count);
        }

        [Test]
        public void TestCanSortProjectsFrom()
        {
            var from = new DateTime(2020, 01, 05);

            var projects = reporter.SortProjectsFrom(from);

            Assert.AreEqual(3, projects.Count);
        }

        [Test]
        public void TestCanSortProjectsTo()
        {
            var to = new DateTime(2020, 01, 22);

            var projects = reporter.SortProjectsTo(to);

            Assert.AreEqual(3, projects.Count);
        }

        [Test]
        public async Task TestCanLogHoursStartStop()
        {
            var project = await proRepo.GetProjectByName("testas3");
            var employee = await empRepo.GetEmployeeByLastName("t");


            tracker.LogHours(employee, project, new TimeSpan(2, 15, 23), "testas");
        }

        [Test]
        public async Task TestCanLogHoursList()
        {
            var project = await proRepo.GetProjectByName("testas3");
            var employee = await empRepo.GetEmployeeByLastName("t");

            var proList = new List<ProjectEntity>();
            proList.Add(project);

            Commit commit = new Commit(employee, "d", 5.661);
            var commList = new List<Commit>();
            commList.Add(commit);

            tracker.LogHours(proList, commList);
        }

       /* [Test]
        public async Task TestIfWorkedMoreThanPossible()
        {
            var project = await proRepo.GetProjectByName("testas3");
            var employee = await empRepo.GetEmployeeByLastName("t");

            var proList = new List<ProjectEntity>();
            proList.Add(project);

            Commit commit = new Commit(employee, "d", 18.61);
            var commList = new List<Commit>();
            commList.Add(commit);

            await tracker.LogHours(proList, commList);

            Assert.Throws<BusinessException>( async () => await tracker.LogHours(proList, commList));
        }
        */
    }
}
