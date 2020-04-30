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
            var employee = await empRepo.GetEmployeeByLastName("t");

            Commit commit = new Commit(employee, "testas", 2);

            var res = await commRepo.InsertCommit(commit);

            Assert.AreEqual(employee.Id, res.Employee);
        }
        [Test]
        public async Task TestCanGetCommitsByEmployee()
        {
            var employee = await empRepo.GetEmployeeByLastName("t");

            await TestCanInsertCommit(); // should insert one commit for spec user

            var res = await commRepo.GetCommitsByEmployee(employee);

            Assert.AreNotEqual(0, res.Count);
        }
        [Test]
        public async Task TestCanAddEmployeeToCommit()
        {
            await TestCanInsertCommit();

            var emp = await empRepo.GetEmployeeByLastName("t");
            var commit = await commRepo.GetCommitByDesc("testas");
       
            await commRepo.AddEmployeeToCommit(commit.Id, emp.Id);

            commit = await commRepo.GetCommitById(commit.Id);

            Assert.AreEqual(commit.Employee, emp.Id);
        }
        [Test]
        public async Task TestCanUpdateEmployeeTimeWorked()
        {
            var emp = await empRepo.GetEmployeeByLastName("t");

            await empRepo.UpdateEmployeeTimeWorked(emp.Id, 10.6);

            emp = await empRepo.GetEmployeeById(emp.Id);

            Assert.AreEqual(emp.TimeWorked, 10.6);
        }
        [Test]
        public async Task TestCanInsertProject()
        {
            var employee = await empRepo.GetEmployeeByLastName("t");

            List<EmployeeEntity> list = new List<EmployeeEntity>{ employee };

            Project project = new Project("test", "des", 200, list);

            var response = await proRepo.InsertProject(project);
            var newPro = await proRepo.GetProjectById(response);

            Assert.AreEqual(response, newPro.Id);

        }
        [Test]
        public async Task TestCanAddCommitToProject()
        {
            var commit = await commRepo.GetCommitByDesc("commit1");

            var project = await proRepo.GetProjectByName("pro3");

            await proRepo.AddCommitToProject(commit.Id, project.Id);

            project = await proRepo.GetProjectById(project.Id);

            Assert.AreNotEqual(0, project.Commits.Count);

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

            Assert.AreNotEqual(0, projects.Count);
        }

        [Test]
        public async Task CheckIfEmployeeCanWorkOnTheProject()
        {
            var emp = new EmployeeEntity { Id = "5e20785d2bd93500011dbf6f" };
            var pro = await proRepo.GetProjectByName("pro3");
            var pro2 = await proRepo.GetProjectById("5e9dd10662f3437598045d51");

            var canWork = tracker.CheckIfEmployeeCanWorkOnTheProject(emp, pro);
            Assert.IsTrue(canWork);
            var cantWork = tracker.CheckIfEmployeeCanWorkOnTheProject(emp, pro2);
            Assert.IsFalse(cantWork);

            var canWork2 = tracker.CheckIfEmployeeCanWorkOnTheProject(emp, pro);
            Assert.IsTrue(canWork2);
            var cantWork2 = tracker.CheckIfEmployeeCanWorkOnTheProject(emp, pro2);
            Assert.IsFalse(cantWork2);
        }
        [Test]
        public async Task CheckForEmployeeOvertime()
        {
            var emp = new EmployeeEntity { Id = "5e20785d2bd93500011dbf6f" };
            var emp2 = new EmployeeEntity { Id = "5e1da23b7762bb0001888e5e" };

            var over = await tracker.CheckForEmployeeOvertime(emp);
            Assert.IsTrue(over);
            var notover = await tracker.CheckForEmployeeOvertime(emp2);
            Assert.IsFalse(notover);
        }


        [Test]
        public void CheckIfEmployeeWorkedMoreThanPossible()
        {
            var comm = new List<Commit> { new Commit { TimeWorked = 40 } };
            var comm2 = new List<Commit> { new Commit { TimeWorked = 4 } };

            var over = tracker.CheckIfEmployeeWorkedMoreThanPossible(comm);
            Assert.IsTrue(over);
            var notover = tracker.CheckIfEmployeeWorkedMoreThanPossible(comm2);
            Assert.IsFalse(notover);
        }


        [Test]
        public void TestCanSortProjectsFrom()
        {
            var from = new DateTime(2020, 01, 05);

            var projects = reporter.SortProjectsFrom(from);

            Assert.AreNotEqual(0, projects.Count);
        }

        [Test]
        public void TestCanSortProjectsTo()
        {
            var to = new DateTime(2020, 01, 22);

            var projects = reporter.SortProjectsTo(to);

            Assert.AreNotEqual(0, projects.Count);
        }

        [Test]
        public async Task TestCanLogHoursStartStop()
        {
            var project = await proRepo.GetProjectByName("pro3");
            var employee = await empRepo.GetEmployeeByLastName("t");

            await tracker.LogHours(employee, project, new TimeSpan(2, 15, 23), "testas");
        }

        [Test]
        public async Task TestCanLogHoursList()
        {
            var project = await proRepo.GetProjectByName("pro3");
            var employee = await empRepo.GetEmployeeByLastName("t");

            var proList = new List<ProjectEntity>{ project };

            Commit commit = new Commit(employee, "d", 5.661);
            var commList = new List<Commit>{ commit };

            await tracker.LogHours(proList, commList);
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
