using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("dateFrom")]
        public DateTime DateFrom { get; set; }
        [JsonProperty("dateTo")]
        public DateTime DateTo { get; set; }
    }
}
