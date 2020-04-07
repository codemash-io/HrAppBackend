using Newtonsoft.Json;
using System;

namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("meetingRoom")]
        public string MeetingRoom { get; set; }
        [JsonProperty("dateFrom")]
        public DateTime DateFrom { get; set; }
        [JsonProperty("dateTo")]
        public DateTime DateTo { get; set; }
    }
}
