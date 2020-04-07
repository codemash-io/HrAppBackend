using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LambdaFunction
{
    class ProcessDTO
    {
        [JsonProperty("organizerId")]
        public string OrganizerId { get; set; }
        [JsonProperty("roomName")]
        public string RoomName { get; set; }
        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        [JsonProperty("participantsIds")]
        public List<string> ParticipantsIds { get; set; }
    }
}
