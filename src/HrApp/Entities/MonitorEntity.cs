using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Models;
using HrApp.Entities;
using Newtonsoft.Json;

namespace HrApp.Domain
{
    public class MonitorEntity : CustomEntity
    {
        [Field("code")]
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
        [Field("model")]
        [JsonProperty("model", NullValueHandling = NullValueHandling.Ignore)]
        public string Model { get; set; }
        [Field("inches")]
        [JsonProperty("inches", NullValueHandling = NullValueHandling.Ignore)]
        public int Inches { get; set; }
    }
}
