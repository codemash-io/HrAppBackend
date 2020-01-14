using System;
using System.Collections.Generic;

namespace HrApp
{
    public class Menu
    {

        public Menu(DateTime lunchDate, Division division, List<EmployeeEntity> employees)
        {
            LunchDate = lunchDate;
            Division = division;
            Employees = employees;
            Status = MenuStatus.Pending;
        }
        public Menu() { }

        public string Id { get; set; }
        public DateTime LunchDate { get; set; }
        public Division Division { get; set; }
        public List<EmployeeEntity> Employees { get; set; }

        public FoodSupplierEntity SupplierEntity { get; set; }
        
        public FoodOrder Order { get; set; } = new FoodOrder();

        public MenuStatus Status { get; set; }
    }
}
