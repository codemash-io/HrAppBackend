using CodeMash.Models;
using System;

namespace HrApp
{
    [Collection("Employees")]
    public class EmployeeEntity : Entity
    {
        [Field("first_name")]
        public string FirstName { get; set; }
        
        [Field("last_name")]
        public string LastName { get; set; }
        
        [Field("division")]
        public string Division { get; set; }

        [Field("role")]
        public int Role { get; set; }

        [Field("budget")]
        public int Budget { get; set; }

        [Field("time_worked")]
        public double TimeWorked { get; set; }

    }
}
