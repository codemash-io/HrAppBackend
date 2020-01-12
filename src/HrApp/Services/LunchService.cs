using System;
using System.Collections.Generic;
using System.Linq;
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
            var closestFriday = DateTime.Now.StartOfWeek(DayOfWeek.Friday).AddHours(12);
            
            var employees = await HrService.GetEmployeesWhoWorksOnLunchDay(division, closestFriday);
            
            var menu = new Menu(closestFriday, division, employees);

            await MenuRepository.InsertMenu(menu);

            return menu;

        }
        
        
        public async Task AdjustMenuLunchTime(DateTime lunchtime, Menu menu)
        {
            if (lunchtime.DayOfWeek == DayOfWeek.Saturday || lunchtime.DayOfWeek == DayOfWeek.Sunday || lunchtime.DayOfWeek == DayOfWeek.Monday)
            {
                throw new BussinessException("Wrong date has been applied because it is a weekend");
            } 
            
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                throw new BussinessException("Today you are not able to set a lunch day");
            }
            
            if (DateTime.Now > lunchtime)
            {
                throw new BussinessException("Menu not created yet");
            }

            await MenuRepository.UpdateMenuLunchTime(lunchtime, menu);
        } 

        public async Task PublishMenu(Menu menu)
        {
            if (DateTime.Now > menu.LunchDate)
            {
                throw new BussinessException("Menu is not valid anymore");
            }
            
            if (menu.SupplierEntity == null)
            {
                throw new BussinessException("supplier is not defined");
            }

            if (menu.Order == null || !menu.Order.Items.Any())
            {
                throw new BussinessException("There is no defined food yet");
            }

            await MenuRepository.AdjustMenuStatus(menu, MenuStatus.InProcess);
        }


        public async Task OrderFood(EmployeeEntity employeeEntity, List<PersonalOrderPreference> preferences, Menu menu)
        {
            if (menu.Status != MenuStatus.InProcess)
            {
                throw new BussinessException("Menu is not available anymore");
            }
            if (preferences == null || !preferences.Any())
            {
                throw new BussinessException("Please provide your wishes");
            }

            await MenuRepository.MakeEmployeeOrder(menu, preferences, employeeEntity);
        }

        public async Task CloseMenu(Menu menu)
        {
            await MenuRepository.AdjustMenuStatus(menu, MenuStatus.Closed);
        }     

        public async Task SendReminderAboutFoodOrder(Menu menu)
        {
            var isLunchTomorrow = IsLunchTomorrow(menu.LunchDate);
            if (!isLunchTomorrow)
            {
                throw new BussinessException("It's too early to send message");
            }

            if (DateTime.Now > menu.LunchDate)
            {
                throw new BussinessException("Order time has passed");
            }

            var employees = await MenuRepository.GetEmployeesWhoOrderedFood(menu);
            
            NotificationSender.SendReminderAboutFoodOrder(employees);

        }
        public async Task SendNotificationThatFoodArrived(Menu menu)
        {
            var employees = await MenuRepository.GetEmployeesWhoOrderedFood(menu);
                       
            NotificationSender.SendNotificationAboutFoodIsArrived(employees);
            
        }

        protected bool IsLunchTomorrow(DateTime lunchtime)
        {
            var difference = (lunchtime - DateTime.Now).Days + 1;
            return difference == 1;
        }

        


    }
}
