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
        public List<MenuItem> MainFood { get; set; } = new List<MenuItem>();
        
        [Field("soup")]
        public List<MenuItem> Soup { get; set; } = new List<MenuItem>();
        
        [Field("drinks")]
        public List<MenuItem> Drinks { get; set; } = new List<MenuItem>();
        
        [Field("souces")]
        public List<MenuItem> Souces { get; set; } = new List<MenuItem>();
    }


    public class Dish
    {
        public int Number { get; set; }
        
        [Field("employees")]
        public List<string> Employees { get; set; }
    }
    
    
    
}