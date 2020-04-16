using Newtonsoft.Json;
using System;

namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
        [JsonProperty("roomName")]
        public string RoomName { get; set; }
        [JsonProperty("dateFrom")]
        public DateTime DateFrom { get; set; }
        [JsonProperty("dateTo")]
        public DateTime DateTo { get; set; }
    }
}
