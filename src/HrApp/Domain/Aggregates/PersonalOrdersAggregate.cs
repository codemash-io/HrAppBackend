using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public class PersonalOrdersAggregate
    {

        [Field("date")]
        public float Date { get; set; }

        [Field("employees")]
        public List<EmployeesWithOrderedMeal> Employees { get; set; }
    }
}
