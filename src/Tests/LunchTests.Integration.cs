using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrApp;
using NSubstitute;
using Xunit;

namespace Tests
{
    public class LunchOrderIntegrations
    {
        
        [Fact]
        public async Task Create_blank_menu()
        {
            var division = new Division {Id = "5d88ae84a792110001fef326"};
            
            var lunchService = new LunchService
            {
                HrService = new HrService { EmployeesRepository = new EmployeesRepository() },
                MenuRepository = new MenuRepository()
            };
            
            var menu = await lunchService.CreateBlankMenu(division);
            
            Assert.Equal("5d88ae84a792110001fef326", menu.Division.Id);
            
        }
        
        [Fact]
        public async Task Send_notifications_to_employees_about_food_order()
        {
            var division = new Division {Id = "5d88ae84a792110001fef326"};
            var notificationSenderMock = Substitute.For<INotificationSender>();
            
            var lunchService = new LunchService
            {
                HrService = new HrService { EmployeesRepository = new EmployeesRepository() },
                MenuRepository = new MenuRepository(),
                NotificationSender = notificationSenderMock
            };
            
            var menu = await lunchService.CreateBlankMenu(division);
            menu.LunchDate = DateTime.Now.AddHours(11);
            var users = await lunchService.SendMessageAboutFoodOrder(menu);
            
            
            Assert.Equal("5d88ae84a792110001fef326", menu.Division.Id);
            Assert.True(users.Count > 0);
            await notificationSenderMock.Received().SendReminderAboutFoodOrder(Arg.Any<List<Guid>>());
            
        }
        
        
        
    }
}
