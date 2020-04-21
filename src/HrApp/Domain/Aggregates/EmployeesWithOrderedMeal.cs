using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public class EmployeesWithOrderedMeal
    {
        [Field("employee")]
        public string Employee { get; set; }

        [Field("main")]
        public string Main { get; set; }
        [Field("soup")]
        public string Soup { get; set; }
        [Field("drink")]
        public string Drink { get; set; }
        [Field("souce")]
        public string Souce { get; set; }
    }
}
