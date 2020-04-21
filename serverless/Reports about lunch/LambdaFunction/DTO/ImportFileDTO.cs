using CodeMash.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaFunction
{
    class ImportFileDTO
    {
        [JsonProperty("_id")]
        public string LunchMenuId { get; set; }
    }
}
