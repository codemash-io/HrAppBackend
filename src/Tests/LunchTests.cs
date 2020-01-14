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

        [Fact]
        public async Task Create_Employee_Order()
        {
            var division = new Division { Id = "5e144e04e39c590001d31fcd" };
            var employee = new EmployeeEntity { Id = "5e1c70f2cafa8c00013d1095" };
            List<PersonalOrderPreference> preferences = new List<PersonalOrderPreference>()
            { new PersonalOrderPreference { Type=FoodType.Main,FoodNumber=2},
            new PersonalOrderPreference { Type=FoodType.Soup,FoodNumber=1},
            new PersonalOrderPreference { Type=FoodType.Drinks,FoodNumber=2},
            };

            ILunchService lunchService = new LunchService
            {
                HrService = new HrService { EmployeesRepository = new EmployeesRepository() },
                MenuRepository = new MenuRepository()
            };


            var client = new CodeMashClient("5ad1qmk9ehm-LmieTRmAObODr4DdVNhi", Guid.Parse("4a988807-b77f-4518-8e4a-d59eaa4da592"));
            var service = new CodeMashRepository<MenuEntity>(client);
            var closestFriday = DateTime.Now.StartOfWeek(DayOfWeek.Friday);
            var menu = new Menu { Id = "5e144e04e39c590001d31fcd", Status = MenuStatus.InProcess };

            await lunchService.OrderFood(employee, preferences, menu);

            // Assert.Equal("5d88ae84a792110001fef326", menu2.Division.Id);

        }


        [Fact]
        public async Task Send_Notification_About_Food_Order_succesfull()
        {


            ILunchService lunchService = new LunchService
            {
                HrService = new HrService { EmployeesRepository = new EmployeesRepository() },
                MenuRepository = new MenuRepository()
            };


            //var client = new CodeMashClient("5ad1qmk9ehm-LmieTRmAObODr4DdVNhi", Guid.Parse("4a988807-b77f-4518-8e4a-d59eaa4da592"));
            // var service = new CodeMashRepository<MenuEntity>(client);
            // var closestFriday = DateTime.Now.StartOfWeek(DayOfWeek.Friday);
            var menu = new Menu { Id = "5e1dad027762bb00018926f9", Status = MenuStatus.InProcess };

            await lunchService.SendReminderAboutFoodOrder(menu);

            // Assert.Equal("5d88ae84a792110001fef326", menu2.Division.Id);

        }

    }
}
