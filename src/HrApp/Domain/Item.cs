using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HrApp.Domain
{
    public class Item
    {
        [Field("price")]
        [JsonProperty("price")]
        public Price Price { get; set; }

        [Field("web_link")]
        [JsonProperty("web_link", NullValueHandling = NullValueHandling.Ignore)]
        public string WebLink { get; set; }

        [Field("description")]
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }
}