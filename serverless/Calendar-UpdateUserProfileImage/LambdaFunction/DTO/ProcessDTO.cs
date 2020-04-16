using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("image")]
        public byte[] ImageBytes { get; set; }
    }
}
