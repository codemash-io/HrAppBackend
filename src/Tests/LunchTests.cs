using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeMash.Client;
using CodeMash.Repository;
using HrApp;
using Xunit;

namespace Tests
{
    public class TestsOfLunchOrderServers
    {
        [Fact]
        public async Task Can_create_blank_menu_for_each_friday()
        {
            var hrServiceMock = Substitute.For<IHrService>();
            hrServiceMock
                .GetEmployeesWhoWorksOnLunchDay(Arg.Any<Division>(), Arg.Any<DateTime>())
                .Returns(new List<EmployeeEntity>());
            
            var repoMock = Substitute.For<IMenuRepository>();

            var closestFriday = DateTime.Now.StartOfWeek(DayOfWeek.Friday);
            
            ILunchService lunchService = new LunchService
            {
                HrService = hrServiceMock,
                MenuRepository = repoMock
            };
            
            var menu = await lunchService.CreateBlankMenu(new Division());

            Assert.Equal(closestFriday.AddHours(12), menu.LunchDate);
            await repoMock.Received().InsertMenu(Arg.Any<Menu>());
        }
        
        [Fact]
        public async Task When_blank_menu_created_employees_are_copied()
        {
            var availableEmployeesFromDivision = new List<EmployeeEntity>
            {
                new EmployeeEntity {Id = "1", FirstName = "EmployeeA"},
                new EmployeeEntity {Id = "2", FirstName = "EmployeeB"},
                new EmployeeEntity {Id = "3", FirstName = "EmployeeC"},
                new EmployeeEntity {Id = "4", FirstName = "EmployeeD"}
            };

            var division = new Division {Name = "Division"};
            
            var hrServiceMock = Substitute.For<IHrService>();
            hrServiceMock
                .GetEmployeesWhoWorksOnLunchDay(Arg.Any<Division>(), Arg.Any<DateTime>())
                .Returns(availableEmployeesFromDivision);
            
            var repoMock = Substitute.For<IMenuRepository>();

            var closestFriday = DateTime.Now.StartOfWeek(DayOfWeek.Friday);
            
            ILunchService lunchService = new LunchService
            {
                HrService = hrServiceMock,
                MenuRepository = repoMock
            };
            
            var menu = await lunchService.CreateBlankMenu(division);

            Assert.Equal(4, menu.Employees.Count);
            Assert.Equal("Division", menu.Division.Name);
            await repoMock.Received().InsertMenu(Arg.Any<Menu>());
        }

        [Fact]
        public async Task Create_blank_menu()
        {
            var division = new Division {Id = "5d88ae84a792110001fef326"};
            
            ILunchService lunchService = new LunchService
            {
                HrService = new HrService { EmployeesRepository = new EmployeesRepository() },
                MenuRepository = new MenuRepository()
            };
            
            var menu = await lunchService.CreateBlankMenu(division);
            
            Assert.Equal("5d88ae84a792110001fef326", menu.Division.Id);
            
        }
        
    }
}
