using Newtonsoft.Json;

namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("apiKey")]
        public string APiKey { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("typeId")]
        public string TypeId { get; set; }
    }
}
