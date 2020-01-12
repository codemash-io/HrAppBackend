using System;
using System.Collections.Generic;
using CodeMash.Models;

namespace HrApp
{
    [CollectionName("LunchMenus")]
    public class MenuEntity : Entity
    {
        public string Status { get; set; } = MenuStatus.Pending.ToString();
        
        [FieldName("planned_lunch_date")]
        public DateTime PlannedDate { get; set; }
        [FieldName("division")]
        public string DivisionId { get; set; }
        [FieldName("employees")]
        public List<string> Employees { get; set; }
        
        [FieldName("main_dish_options")]
        public List<string> MainFood { get; set; } = new List<string>();
        
        [FieldName("soup")]
        public List<string> Soup { get; set; } = new List<string>();
    }


    public class Dish
    {
        public int Number { get; set; }
        
        [FieldName("employees")]
        public List<string> Employees { get; set; }
    }
    
    
    
}