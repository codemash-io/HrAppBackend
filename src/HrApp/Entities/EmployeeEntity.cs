using CodeMash.Models;
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
        [Field("address")]
        public string Address { get; set; }

        [Field("employees")]
        public List<string> Employees { get; set; }
        
        [Field("application_user")]
        public Guid UserId { get; set; }

        [Field("business_trips")]
        public List<Trip> BussinessTrips { get; set; }

        [Field("personal_identification_number")]
        public string PersonalId { get; set; }

        [Field("manager")]
        public string ManagerId { get; set; }
        [Field("business_email")]
        public string BusinessEmail { get; set; }
    }

 }