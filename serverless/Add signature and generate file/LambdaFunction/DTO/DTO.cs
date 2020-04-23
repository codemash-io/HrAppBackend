using CodeMash.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaFunction
{
    class DTO
    {
        [JsonProperty("_id")]
        public string AbsenceRequestId { get; set; }
    }
}
