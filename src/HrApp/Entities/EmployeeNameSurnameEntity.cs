using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    [Collection("pc-employees")]
    //[Collection("pc-employees-testing")]
    public class EmployeeNameSurnameEntity : Entity
    {

        [Field("first_name")]
        public string FirstName { get; set; } = "";

        [Field("last_name")]
        public string LastName { get; set; } = "";

        [Field("division")]
        public string Division { get; set; } = "";

        [Field("address")]
        public string Address { get; set; } = "";
        [Field("application_user")]
        public Guid UserId { get; set; } = new Guid();

        [Field("personal_identification_number")]
        public string PersonalId { get; set; } = "";

        [Field("manager")]
        public string ManagerId { get; set; } = "";
        [Field("business_email")]
        public string BusinessEmail { get; set; } = "";
        [Field("number_of_holidays_left")]
        public int NumberOfHolidays { get; set; } = 0;  
       
    }
}
