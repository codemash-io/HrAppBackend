using NSubstitute;
using System.Threading.Tasks;
using HrApp;
using System;
using NUnit.Framework;
using HrApp.Contracts;
using HrApp.Domain;
using HrApp.Entities;
using HrApp.Repositories;
using HrApp.Services;
using System.Collections.Generic;

namespace Tests
{
    public class TimeTrackerTests
    {
        ITimeTrackerService trackerMock;
        ICommitRepository commitMock;
        IProjectRepository projectMock;
        IEmployeesRepository employeeMock;

        [SetUp]
        public void Setup()
        {
            trackerMock = Substitute.For<ITimeTrackerService>();
            commitMock = Substitute.For<ICommitRepository>();
            projectMock = Substitute.For<IProjectRepository>();
            employeeMock = Substitute.For<IEmployeesRepository>();
        }


        [Test]
        public async Task TestCanLogHoursStartStop()
        {
            var commit = new CommitEntity { Id = "1", Description = "test2" };

            List<string> list = new List<string>();
            list.Add("20");

            EmployeeEntity emp = new EmployeeEntity { Id = "20" };
            ProjectEntity pro = new ProjectEntity { Employees = list, Id = "2" };
         

            ITimeTrackerService trackerService = new TimeTrackerService
            {
                CommitRepository = commitMock,
                ProjectRepository = projectMock,
                EmployeeRepository = employeeMock
            };

            commitMock.InsertCommit(Arg.Any<Commit>()).Returns(commit);

            trackerMock.CheckIfEmployeeCanWorkOnTheProject(
                Arg.Any<EmployeeEntity>(), Arg.Any<ProjectEntity>()).Returns(true);


            await trackerService.LogHours(emp, pro, new TimeSpan(2, 2, 2), "desc");



            await commitMock.Received().InsertCommit(Arg.Any<Commit>());
            await projectMock.Received().AddCommitToProject(Arg.Any<string>(), Arg.Any<string>());
            await employeeMock.Received().UpdateEmployeeTimeWorked(Arg.Any<string>(), Arg.Any<double>());

        }

        [Test]
        public async Task TestCanLogMultipleHours()
        {
            var commitsList = new List<Commit>
            {
                new Commit {Id = "1", Description = "DescriptionA", Employee = new EmployeeEntity { Id = "20"}, TimeWorked = 1 },
                new Commit {Id = "2", Description = "DescriptionB", Employee = new EmployeeEntity { Id = "20"}, TimeWorked = 2 },
                new Commit {Id = "3", Description = "DescriptionC", Employee = new EmployeeEntity { Id = "20"}, TimeWorked = 3 },
                new Commit {Id = "4", Description = "DescriptionD", Employee = new EmployeeEntity { Id = "20"}, TimeWorked = 4 }
            };

            List<string> list = new List<string>();
            list.Add("20");

            var c = new CommitEntity { Id = "15" };

            var projectsList = new List<ProjectEntity>
            {
                new ProjectEntity {Id = "1", Name = "ProjectA", Employees = list},
                new ProjectEntity {Id = "2", Name = "ProjectB", Employees = list},
                new ProjectEntity {Id = "3", Name = "ProjectC", Employees = list},
                new ProjectEntity {Id = "4", Name = "ProjectD", Employees = list}
            };

            commitMock.InsertCommit(Arg.Any<Commit>()).Returns(c);

            trackerMock.CheckIfEmployeeCanWorkOnTheProject(
                Arg.Any<EmployeeEntity>(), Arg.Any<ProjectEntity>()).Returns(true);

            trackerMock.CheckIfEmployeeWorkedMoreThanPossible(Arg.Any<List<Commit>>());


            ITimeTrackerService trackerService = new TimeTrackerService
            {
                CommitRepository = commitMock,
                ProjectRepository = projectMock,
                EmployeeRepository = employeeMock
            };

            await trackerService.LogHours(projectsList, commitsList);


            await commitMock.Received().InsertCommit(Arg.Any<Commit>());
            await projectMock.Received().AddCommitToProject(Arg.Any<string>(), Arg.Any<string>());
            await employeeMock.Received().UpdateEmployeeTimeWorked(Arg.Any<string>(), Arg.Any<double>());
        }
      

    }
}
