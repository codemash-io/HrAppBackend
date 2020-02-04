using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HrApp.Domain
{
    public class Price
    {
        [JsonProperty("value")]
        public int Value { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}