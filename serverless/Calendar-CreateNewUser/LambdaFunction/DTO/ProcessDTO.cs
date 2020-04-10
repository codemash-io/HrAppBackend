using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
