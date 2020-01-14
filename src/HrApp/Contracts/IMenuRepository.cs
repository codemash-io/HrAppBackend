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
        Task<List<Guid>> GetEmployeesWhoOrderedFood(Menu menu);
        Task<List<Guid>> GetEmployeesWhoStillNotMadeAnOrder(Menu menu);
    }
}