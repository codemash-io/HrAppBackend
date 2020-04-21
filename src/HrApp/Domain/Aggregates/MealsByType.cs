using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public class MealsByType
    {

        [Field("type")]
        public string Type { get; set; }
        [Field("meals")]
        public List<LunchMenuMeal> Meals { get; set; }
    }
}
