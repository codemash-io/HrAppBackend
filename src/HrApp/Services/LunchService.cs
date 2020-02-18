using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace HrApp
{
    public class LunchService : ILunchService
    {
        public IHrService HrService { get; set; }
        public IMenuRepository MenuRepository { get; set; }
        public INotificationSender NotificationSender { get; set; }
        
        public async Task<Menu> CreateBlankMenu(Division division)
        {
            if (division == null)
            {
                throw new ArgumentNullException(nameof(division), "No division defined");
            }
            
            // TODO: Check if Friday is not a day-off in that Division? 
            var closestFriday = DateTimeExtensions.StartOfWeek(DateTime.Now, DayOfWeek.Friday).AddHours(12);
            
            var employees = await HrService.GetEmployeesWhoWorksOnLunchDay(division, closestFriday);
            
            var menu = new Menu(closestFriday, division, employees);

            var menuId = await MenuRepository.InsertMenu(menu);
            menu.Id = menuId;

            return menu;

        }
        
        
        public async Task AdjustMenuLunchTime(DateTime lunchtime, Menu menu, List<string> PreviousDateEMployees)
        {
            /*if (lunchtime.DayOfWeek == DayOfWeek.Saturday || lunchtime.DayOfWeek == DayOfWeek.Sunday || lunchtime.DayOfWeek == DayOfWeek.Monday)
            {
                throw new BusinessException("Wrong date has been applied because it is a weekend");
            } 
            
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                throw new BusinessException("Today you are not able to set a lunch day");
            }
            */
            if (DateTime.Now > lunchtime)
            {
                await MenuRepository.AdjustMenuStatus(menu, MenuStatus.Canceled);
                
               // throw new BusinessException("Menu not created yet");
            }
            await MenuRepository.CleanOrders(menu);

            menu.Employees = await HrService.GetEmployeesWhoWorksOnLunchDay(menu.Division, lunchtime);                              
            await MenuRepository.UpdateMenuLunchTime(lunchtime, menu);

            var newDateAllEmployees = menu.Employees.Select(x => x.Id).ToList();
            if (IsLunchDayInNearest2Days(lunchtime))
            {
                var employees = await MenuRepository.GetEmployeesWhoAreNewInMenu(menu, PreviousDateEMployees, newDateAllEmployees);
                await NotificationSender.SendNotificationForNewReceiversAboutLunchDate(employees, lunchtime);            
            }       

        } 

        public async Task PublishMenu(Menu menu)
        {
            if (DateTime.Now > menu.LunchDate)
            {
                throw new BusinessException("Menu is not valid anymore");
            }
            
            if (menu.SupplierEntity == null)
            {
                throw new BusinessException("supplier is not defined");
            }

            if (menu.Order == null || !menu.Order.Items.Any())
            {
                throw new BusinessException("There is no defined food yet");
            }

            await MenuRepository.AdjustMenuStatus(menu, MenuStatus.InProcess);
        }


        public async Task OrderFood(EmployeeEntity employeeEntity, List<PersonalOrderPreference> preferences, Menu menu)
        {
            if (employeeEntity == null || string.IsNullOrEmpty(employeeEntity.Id))
            {
                throw new BusinessException("Employee is not provided");
            }
            
            if (preferences == null || !preferences.Any())
            {
                throw new BusinessException("Please provide your wishes");
            }
            
            if (menu == null)
            {
                throw new BusinessException("Menu is not provided");
            }
            
            if (menu.Status != MenuStatus.InProcess)
            {
                throw new BusinessException("Menu is not available anymore");
            }
            
            

            await MenuRepository.MakeEmployeeOrder(menu, preferences, employeeEntity);
        }

        public async Task CloseMenu(Menu menu)
        {
            await MenuRepository.AdjustMenuStatus(menu, MenuStatus.Closed);
        }

        public async Task<List<Guid>> SendMessageAboutFoodOrder(Menu menu)
        {
            var isLunchTomorrow = IsLunchTodayOrTomorrow(menu.LunchDate);
            if (!isLunchTomorrow)
            {
                throw new BusinessException("It's too early to send message");
            }

            if (DateTime.Now > menu.LunchDate)
            {
                throw new BusinessException("Order time has passed");
            }

            var employees = await MenuRepository.GetEmployeesWhoCanOrderFood(menu);
            
            await NotificationSender.SendReminderAboutFoodOrder(employees, menu.LunchDate);

            return employees;
        }

        public async Task SendReminderAboutFoodOrder(Menu menu)
        {
            if (menu.LunchDate < DateTime.Now)
            {
                throw new BusinessException("Order time has passed");
            }
            
            var isLunchTomorrow = IsLunchTodayOrTomorrow(menu.LunchDate);
            if (!isLunchTomorrow)
            {
                throw new BusinessException("It's too early to send message");
            }

            var employees = await MenuRepository.GetEmployeesWhoCanOrderFood(menu);
            
            await NotificationSender.SendReminderAboutFoodOrder(employees, menu.LunchDate);

        }
        public async Task SendNotificationThatFoodArrived(Menu menu)
        {
            var employees = await MenuRepository.GetEmployeesWhoCanOrderFood(menu);
                       
            await NotificationSender.SendNotificationAboutFoodIsArrived(employees);
            
        }

        private bool IsLunchTodayOrTomorrow(DateTime lunchtime)
        {
            var difference = lunchtime.Subtract(DateTime.Now).TotalHours;
            return difference <= 26;
        }

        private bool IsLunchDayInNearest2Days(DateTime lunchtime)
        {            
            var difference = lunchtime.Subtract(DateTime.Now).TotalHours;
            return difference <= 50;
        }
        


    }
}
