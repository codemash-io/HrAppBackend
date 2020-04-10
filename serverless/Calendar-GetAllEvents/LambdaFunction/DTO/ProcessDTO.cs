using Newtonsoft.Json;
using System;

namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("roomName")]
        public string RoomName { get; set; }
        [JsonProperty("dateFrom")]
        public DateTime DateFrom { get; set; }
        [JsonProperty("dateTo")]
        public DateTime DateTo { get; set; }
    }
}
