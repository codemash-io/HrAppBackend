using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public class EmergencyContact
    {
        [Field("name")]
        public string Name { get; set; }
        [Field("position")]
        public string Position { get; set; }
        [Field("phone")]
        public string Phone { get; set; }

    }
}
