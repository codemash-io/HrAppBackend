using Newtonsoft.Json;
using System;

namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("dateFrom")]
        public DateTime DateFrom { get; set; }
        [JsonProperty("dateTo")]
        public DateTime DateTo { get; set; }
    }
}
