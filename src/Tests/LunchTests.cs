using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrApp;
using NSubstitute.Core.Arguments;
using Xunit;

namespace Tests
{
    public class LunchOrder
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
            
            var lunchService = new LunchService
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
            
            var lunchService = new LunchService
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
        public async Task Send_notification_to_employees_who_can_order_the_food()
        {
            // Arrange
            var availableEmployeesFromDivision = new List<EmployeeEntity>
            {
                new EmployeeEntity {Id = "1", FirstName = "EmployeeA", UserId = Guid.NewGuid()},
                new EmployeeEntity {Id = "2", FirstName = "EmployeeB", UserId = Guid.NewGuid()},
                new EmployeeEntity {Id = "3", FirstName = "EmployeeC", UserId = Guid.NewGuid()},
                new EmployeeEntity {Id = "4", FirstName = "EmployeeD", UserId = Guid.NewGuid()}
            };
            
            var division = new Division {Name = "Division"};
            var menu = new Menu(DateTime.Now.AddDays(1), division, availableEmployeesFromDivision);
            
            var repoMock = Substitute.For<IMenuRepository>();
            repoMock.GetEmployeesWhoCanOrderFood(Arg.Any<Menu>())
                .Returns(availableEmployeesFromDivision.Select(x => x.UserId).ToList());

            var notificationSenderMock = Substitute.For<INotificationSender>();
            
            var lunchService = new LunchService
            {
                MenuRepository = repoMock,
                NotificationSender = notificationSenderMock
            };
            
            // Act
            await lunchService.SendReminderAboutFoodOrder(menu);
                
            // Assert
            await notificationSenderMock.Received().SendReminderAboutFoodOrder(Arg.Any<List<Guid>>(), Arg.Any<DateTime>());
            
        }
        
        [Fact]
        public async Task Throws_exception_if_reminder_is_to_early()
        {
            // Arrange
            var division = new Division {Name = "Division"};
            var menu = new Menu(DateTime.Now.AddHours(27), division, null);

            var lunchService = new LunchService();
            
            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await lunchService.SendReminderAboutFoodOrder(menu));
            
            // Assert
            Assert.Equal("It's too early to send message", exception.Message);

        }
        
        [Fact]
        public async Task Throws_exception_if_reminder_is_in_the_past_date()
        {
            // Arrange
            var division = new Division {Name = "Division"};
            var menu = new Menu(DateTime.Now.AddHours(-1), division, null);

            var lunchService = new LunchService();
            
            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await lunchService.SendReminderAboutFoodOrder(menu));
            
            // Assert
            Assert.Equal("Order time has passed", exception.Message);

        }
        
    }
}
