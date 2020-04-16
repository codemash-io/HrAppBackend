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
            var division = new Division {Id = "5e144e04e39c590001d31fcd" };
            
            var lunchService = new LunchService
            {
                HrService = new HrService { EmployeesRepository = new EmployeesRepository() },
                MenuRepository = new MenuRepository()
            };
            
            var menu = await lunchService.CreateBlankMenu(division);
            
            Assert.Equal("5e144e04e39c590001d31fcd", menu.Division.Id);
            
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
            await notificationSenderMock.Received().SendReminderAboutFoodOrder(Arg.Any<List<Guid>>(), Arg.Any<DateTime>());
            
        }
        
        
        [Fact]
        public async Task Get_closest_menu()
        {
            var repo = new MenuRepository();
            var menu = await repo.GetClosestMenu();
            
            Assert.NotNull(menu);
           
        }
        
        [Fact]
        public async Task Get_employees_who_ordered_the_food()
        {
            var repo = new MenuRepository();
            var menu = await repo.GetClosestMenu();
            var employees = await repo.GetEmployeesWhoOrderedFood(menu);
            
            Assert.True(employees.Count > 0);
           
        }
        
        [Fact]
        public async Task Make_food_order()
        {
            var repo = new MenuRepository();
            var menu = await repo.GetClosestMenu();
            var employees = await repo.GetEmployeesWhoOrderedFood(menu);
            
            Assert.True(employees.Count > 0);
           
        }
        
        [Fact]
        public async Task Make_food_order_for_employee()
        {
            var repo = new MenuRepository();
            var menu = await repo.GetClosestMenu();

            var employee = new EmployeeEntity {Id = "5d8ca4aa07f6fe000114ba01"};

            var preferences = new List<PersonalOrderPreference>()
            {
                new PersonalOrderPreference() {Type = FoodType.Main, FoodNumber = 1},
                new PersonalOrderPreference() {Type = FoodType.Soup, FoodNumber = 1},
                new PersonalOrderPreference() {Type = FoodType.Drinks, FoodNumber = 1},
                new PersonalOrderPreference() {Type = FoodType.Souce, FoodNumber = 1},
            };
            
            await repo.MakeEmployeeOrder(menu, preferences, employee);
           
        }
        // preferences : [{ Type : "Main", FoodNumber : 1 },]
        [Fact]
        public async Task Get_Employees_Who_Works_On_Lunch_Day()
        {
            var repo = new HrService() {
                EmployeesRepository = new EmployeesRepository()
            };
            var division = new Division() { Id = "5e144e04e39c590001d31fcd" };
            var employees = await repo.GetEmployeesWhoWorksOnLunchDay(division,new DateTime(2020,02,13).Date);
            Assert.True(employees.Count > 0);
        }

        [Fact]
        public async Task Adjust_Lunch_day_Test()
        {

            var division = new Division() { Id = "5e144e04e39c590001d31fcd" };
            var repo = new LunchService()
            {
                HrService = new HrService() { EmployeesRepository = new EmployeesRepository()},
                MenuRepository = new MenuRepository(),
                NotificationSender = new NotificationSender()
            };
            var closestFriday = DateTime.Now.StartOfWeek(DayOfWeek.Friday).AddHours(12);
            var menu = new Menu(closestFriday, division, new List<EmployeeEntity>()) { Id = "5e3c016f214efe00018721ab" };
            var previousDateEmployees = new List<string>() { "5e3aca299ae79b0001a9db38", "5e3aca1b9ae79b0001a9db13" };
            await repo.AdjustMenuLunchTime(new DateTime(2020, 02, 19), menu, previousDateEmployees);
            
        }

        [Fact]
        public async Task LunchOrderReportTest()
        {
            var repo = new LunchService()
            {
                HrService = new HrService() { EmployeesRepository = new EmployeesRepository() },
                MenuRepository = new MenuRepository(),
                NotificationSender = new NotificationSender(),
                AggregateRepository = new AggregateRepository(),
                FileRepository = new FileRepository()
            };
            await repo.FormatReportsOnLunchOrders("5e6a1b980187c000015b0767");
        }
    }
}
