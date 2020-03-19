using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    [Collection("absence-type")]
    class AbsenceType:Entity
    {
        public string name { get; set; }
    }
}
