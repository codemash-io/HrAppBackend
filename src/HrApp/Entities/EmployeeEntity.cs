using System;
using System.Collections.Generic;
using CodeMash.Models;

namespace HrApp
{
    [Collection("pc-employees")]
    public class EmployeeEntity : Entity
    {
        [Field("first_name")]
        public string FirstName { get; set; }
        
        [Field("last_name")]
        public string LastName { get; set; }
        
        [Field("division")]
        public string Division { get; set; }
        
        [Field("employees")]
        public List<string> Employees { get; set; }
        
        [Field("application_user")]
        public Guid UserId { get; set; }
    }
}
