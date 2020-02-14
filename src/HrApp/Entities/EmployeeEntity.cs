
﻿using CodeMash.Models;
using System;
 using System.Collections.Generic;

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

        [Field("role")]
        public int Role { get; set; }

        [Field("budget")]
        public int Budget { get; set; }

        [Field("time_worked")]
        public double TimeWorked { get; set; }
        
        [Field("employees")]
        public List<string> Employees { get; set; }
        
        [Field("application_user")]
        public Guid UserId { get; set; }

    }
 }
