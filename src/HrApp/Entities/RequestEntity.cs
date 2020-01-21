using System.Collections.Generic;
using System.Linq;
using CodeMash.Models;

namespace HrApp
{
    [Collection("requests")]
    public class RequestEntity : Entity
    {
        [Field("description")]
        public string Description { get; set; }

        [Field("price")]
        public int Price { get; set; }

        //[Field("online_links")]
        //public List<string> Links { get; set; }

        [Field("request_type")]
        public string RequestType { get; set; }
    }
}