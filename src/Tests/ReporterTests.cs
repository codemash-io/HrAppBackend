using NSubstitute;
using System.Threading.Tasks;
using HrApp;
using System;
using NUnit.Framework;
using HrApp.Contracts;
using HrApp.Domain;
using HrApp.Entities;
using HrApp.Services;
using System.Collections.Generic;

namespace Tests
{
    class ReporterTests
    {
        ITimeTrackerService trackerMock;
        ICommitRepository commitMock;
        IProjectRepository projectMock;
        IEmployeesRepository employeeMock;
        IReportsService reportMock;

        [SetUp]
        public void Setup()
        {
            trackerMock = Substitute.For<ITimeTrackerService>();
            commitMock = Substitute.For<ICommitRepository>();
            projectMock = Substitute.For<IProjectRepository>();
            employeeMock = Substitute.For<IEmployeesRepository>();
            reportMock = Substitute.For<IReportsService>();
        }


        [Test]
        public async Task TestCanSortProjectsFromTo()
        {
            List<string> list = new List<string>();
            list.Add("20");

            var projectsList = new List<ProjectEntity>
            {
                new ProjectEntity {Id = "1", Name = "ProjectA", Employees = list},
                new ProjectEntity {Id = "2", Name = "ProjectB", Employees = list},
                new ProjectEntity {Id = "3", Name = "ProjectC", Employees = list},
                new ProjectEntity {Id = "4", Name = "ProjectD", Employees = list}
            };

            IReportsService reportService = new ReportsService
            {
                projectRepository = projectMock
            };

            projectMock.SortProjects(Arg.Any<DateTime>(), Arg.Any<DateTime>())
                .Returns(projectsList);

            var res = reportService.SortProjects(new DateTime(2020,01,05), new DateTime(2020, 01, 05));

            Assert.AreEqual(projectsList.Count, res.Count);
            Assert.AreEqual(projectsList[0].Name, res[0].Name);
            await projectMock.Received().SortProjects(Arg.Any<DateTime>(), Arg.Any<DateTime>());

        }

        [Test]
        public async Task TestCanSortProjectsFrom()
        {
            List<string> list = new List<string>();
            list.Add("20");

            var projectsList = new List<ProjectEntity>
            {
                new ProjectEntity {Id = "1", Name = "ProjectA", Employees = list},
                new ProjectEntity {Id = "2", Name = "ProjectB", Employees = list},
                new ProjectEntity {Id = "3", Name = "ProjectC", Employees = list}
            };

            IReportsService reportService = new ReportsService
            {
                projectRepository = projectMock
            };

            projectMock.SortProjectsFrom(Arg.Any<DateTime>())
                .Returns(projectsList);

            var res = reportService.SortProjectsFrom(new DateTime(2020, 01, 05));

            Assert.AreEqual(projectsList.Count, res.Count);
            Assert.AreEqual(projectsList[0].Name, res[0].Name);
            await projectMock.Received().SortProjectsFrom(Arg.Any<DateTime>());

        }

        [Test]
        public async Task TestCanSortProjectsTo()
        {
            List<string> list = new List<string>();
            list.Add("20");

            var projectsList = new List<ProjectEntity>
            {
                new ProjectEntity {Id = "1", Name = "ProjectA", Employees = list},
                new ProjectEntity {Id = "3", Name = "ProjectC", Employees = list}
            };

            IReportsService reportService = new ReportsService
            {
                projectRepository = projectMock
            };

            projectMock.SortProjectsTo(Arg.Any<DateTime>())
                .Returns(projectsList);

            var res = reportService.SortProjectsTo(new DateTime(2020, 01, 15));

            Assert.AreEqual(projectsList.Count, res.Count);
            Assert.AreEqual(projectsList[0].Name, res[0].Name);
            await projectMock.Received().SortProjectsTo(Arg.Any<DateTime>());

        }

        [Test]
        public void TestCanEmployeeAccessReports()
        {
            EmployeeEntity emp = new EmployeeEntity { Role = 2 };

            IReportsService reportService = new ReportsService
            {
                projectRepository = projectMock
            };

            var res = reportService.CanEmployeeAccessReports(emp);

            Assert.IsTrue(res);
        }
        [Test]
        public void TestCanGetProjectDetails()
        {
            List<string> list = new List<string>();
            list.Add("20");
            ProjectEntity pro = new ProjectEntity { Employees = list, Id = "2", Name ="pro1" };

            IReportsService reportService = new ReportsService
            {
                projectRepository = projectMock
            };

            projectMock.GetProjectByName(Arg.Any<string>()).Returns(pro);

            var res = reportService.GetProjectDetails("pro1");

            Assert.AreEqual(pro.Name, res.Name);
            projectMock.Received().GetProjectByName(Arg.Any<string>());
        }
    }
}
