using Microsoft.Graph;
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
        [JsonProperty("contactId")]
        public string ContactId { get; set; }
        [JsonProperty("contact")]
        public Contact Contact { get; set; }
    }
}
