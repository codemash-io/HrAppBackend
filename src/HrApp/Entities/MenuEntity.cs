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
        public List<Dish> MainFood { get; set; } = new List<Dish>();
        
        [FieldName("soups")]
        public List<Dish> Soup { get; set; } = new List<Dish>();
        [FieldName("drinks")]
        public List<Dish> Drink { get; set; } = new List<Dish>();
        [FieldName("souces")]
        public List<Dish> Souce { get; set; } = new List<Dish>();
    }


    public class Dish
    {
        [FieldName("no")]
        public int Number { get; set; }
        
        [FieldName("employees")]
        public List<string> Employees { get; set; }
    }
    
    
    
}