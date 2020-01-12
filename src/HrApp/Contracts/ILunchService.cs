using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public interface ILunchService
    {
        INotificationSender NotificationSender { get; set; }

        IHrService HrService { get; set; }

        /// <summary>
        /// automatically each monday (add only these employees who is not in holiday)
        /// </summary>
        /// <returns></returns>
        public Task<Menu> CreateBlankMenu(Division division); 

        
        /// <summary>
        /// Change Menu time and renew employee list who are working in day which are set.
        /// </summary>
        /// <param name="lunchtime"></param>
        /// <param name="menu"></param>
        public Task AdjustMenuLunchTime(DateTime lunchtime, Menu menu);
        
        /// <summary>
        /// Publish Menu
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public Task PublishMenu(Menu menu);    
        
        /// <summary>
        /// Order food for employee
        /// </summary>
        /// <param name="employeeEntity"></param>
        /// <param name="preferences"></param>
        /// <param name="menu"></param>
        public Task OrderFood(EmployeeEntity employeeEntity, List<PersonalOrderPreference> preferences, Menu menu);
        
        /// <summary>
        /// Menu is closed and additional orders should be declined 
        /// </summary>
        /// <param name="menu"></param>
        public Task CloseMenu(Menu menu);

        /// <summary>
        /// Sends reminder about upcoming food delivery.
        /// </summary>
        /// <param name="menu"></param>
        public Task SendReminderAboutFoodOrder(Menu menu);

        /// <summary>
        /// Notify recipients about food is in the kitchen
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public Task SendNotificationThatFoodArrived(Menu menu);

    }
}
