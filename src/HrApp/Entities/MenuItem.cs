using System.Collections.Generic;
using CodeMash.Models;

namespace HrApp
{
    public class MenuItem
    {
        [Field("title")]
        public string Food { get; set; }
        //[Field("price")]
        //public double Price { get; set; }
        [Field("no")]
        public int No { get; set; }
        [Field("employees")]
        public List<string> Employees { get; set; }
    }
}