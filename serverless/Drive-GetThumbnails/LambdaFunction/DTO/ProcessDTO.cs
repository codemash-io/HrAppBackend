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
        public string TypeId { get; set; } = null;
        [JsonProperty("itemId")]
        public string ItemId { get; set; }
        [JsonProperty("select")]
        public string Select { get; set; } = null;
    }
}
