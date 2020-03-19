using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaFunction
{
    class Absence
    {
        [JsonProperty("_id")]
        public string AbsenceId { get; set; }
        [JsonProperty("employee")]
        public string EmployeeId { get; set; }
    }
}
