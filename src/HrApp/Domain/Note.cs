using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ServiceStack;

namespace HrApp.Domain
{
    public class Note
    {
        [Field("note")]
        [JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
        public string CustomNote { get; set; }

        [Field("created_on")]
        [JsonProperty("created_on", NullValueHandling = NullValueHandling.Ignore)]
        public float CreatedOn { get; set; }
    }
}
