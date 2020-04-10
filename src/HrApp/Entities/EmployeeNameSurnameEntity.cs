using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    [Collection("pc-employees-testing")]
    public class EmployeeNameSurnameEntity : Entity
    {

        [Field("first_name")]
        public string FirstName { get; set; } = "";

        [Field("last_name")]
        public string LastName { get; set; } = "";


    }
}
