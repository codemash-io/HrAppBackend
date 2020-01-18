using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IMenuRepository
    {
        Task<string> InsertMenu(Menu menu);

        Task UpdateMenuLunchTime(DateTime newTime, Menu menu);

        Task MakeEmployeeOrder(Menu menu, List<PersonalOrderPreference> preferences, EmployeeEntity employeeEntity);

        Task AdjustMenuStatus(Menu menu, MenuStatus status);

        /// <summary>
        /// Gets all employees who can order the food and haven't done it yet
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        Task<List<Guid>> GetEmployeesWhoCanOrderFood(Menu menu);
        
        /// <summary>
        /// Gets all employees who made an order.
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        Task<List<Guid>> GetEmployeesWhoOrderedFood(Menu menu);

        /// <summary>
        /// Checks all menus which has state InProcess and gets with the lowest date.
        /// </summary>
        /// <returns></returns>
        Task<Menu> GetClosestMenu();
    }
}