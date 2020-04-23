using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
        [JsonProperty("eventId")]
        public string EventId { get; set; }
        [JsonProperty("roomName")]
        public string RoomName { get; set; }
    }
}
