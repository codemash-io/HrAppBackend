using System;
using System.Collections.Generic;
using CodeMash.Models;

namespace HrApp
{
    [Collection("LunchMenus")]
    public class MenuEntity : Entity
    {
        public string Status { get; set; } = MenuStatus.Pending.ToString();
        
        [Field("planned_lunch_date")]
        public DateTime PlannedDate { get; set; }
        [Field("division")]
        public string DivisionId { get; set; }
        [Field("employees")]
        public List<string> Employees { get; set; }
        
        [Field("main_dish_options")]
        public List<string> MainFood { get; set; } = new List<string>();
        
        [Field("soup")]
        public List<string> Soup { get; set; } = new List<string>();
    }


    public class Dish
    {
        public int Number { get; set; }
        
        [Field("employees")]
        public List<string> Employees { get; set; }
    }
    
    
    
}