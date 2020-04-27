using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("apiKey")]
        public string APiKey { get; set; }
        [JsonProperty("emailsToSend")]
        public List<string> Emails { get; set; }
    }
}
