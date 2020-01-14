using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp.Domain
{
    public class Employee
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Division { get; set; }
        public int Role { get; set; } //1-developer 2- manager
        // timeworked  should be set to zero at the start of each the month
        public double TimeWorked { get; set; } //employee actual work time during month
        public int Budget { get; set; } //monthly budget in hours (160h when full time)  

        public Employee(string firstName, string lastName, string division, int budget, int role)
        {
            FirstName = firstName;
            LastName = lastName;
            Division = division;
            Budget = budget;
            Role = role;
            TimeWorked = 0;
        }
    }    
}
