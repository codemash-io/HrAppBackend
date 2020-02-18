using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Models;
using Newtonsoft.Json;

namespace HrApp.Domain
{
    public class CashDTO
    {
        public Price Amount { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
    }
}
