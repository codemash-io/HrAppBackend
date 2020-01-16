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
        public List<string> Employees { get; set; } = new List<string>();
        
        [Field("main_dish_options")]
        public List<Dish> MainFood { get; set; } = new List<Dish>();
        
        [Field("soups")]
        public List<Dish> Soup { get; set; } = new List<Dish>();
        [Field("drinks")]
        public List<Dish> Drink { get; set; } = new List<Dish>();
        [Field("souces")]
        public List<Dish> Souce { get; set; } = new List<Dish>();
    }


    public class Dish
    {
        [Field("no")]
        public int Number { get; set; }
        
        [Field("employees")]
        public List<string> Employees { get; set; }
    }
    
    
    
}