using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Models;
using Newtonsoft.Json;

namespace HrApp.Domain
{
    public class TripDTO
    {
        public string Country { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
