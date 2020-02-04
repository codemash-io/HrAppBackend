using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HrApp.Domain
{
    public class Cash
    {
        [Field("amount")]
        [JsonProperty("amount")]
        public Price Amount { get; set; }

        [Field("date")]
        [JsonProperty("date")]
        public float Date { get; set; }

        [Field("description")]
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
