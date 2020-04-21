using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public class LunchMenuMeal
    {
        [Field("title")]
        public string Title { get; set; }
        [Field("employeeCount")]
        public int EmployeeCount { get; set; }
    }
}
