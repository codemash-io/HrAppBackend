using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    [Collection("pc-employees")]
    //[Collection("pc-employeess-testing")]
    public class EmployeeNameSurnameEntity : Entity
    {
        [Field("first_name")]
        public string first_name { get; set; } = "";

        [Field("last_name")]
        public string last_name { get; set; } = "";

        
       
    }
}
