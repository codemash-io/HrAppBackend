using CodeMash.Models;
using System;

namespace HrApp
{
    [CollectionName("pc-employees")]
    public class EmployeeEntity : Entity
    {
        [FieldName("first_name")]
        public string FirstName { get; set; }
        
        [FieldName("last_name")]
        public string LastName { get; set; }
        
        [FieldName("division")]
        public string Division { get; set; }

        [FieldName("role")]
        public int Role { get; set; }

        [FieldName("budget")]
        public int Budget { get; set; }

        [FieldName("time_worked")]
        public TimeSpan TimeWorked { get; set; }

    }
}
