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

        [JsonProperty("expand")]
        public string Expand { get; set; } = null;
        [JsonProperty("select")]
        public string Select { get; set; } = null;
        [JsonProperty("skipToken")]
        public string SkipToken { get; set; } = null;
        [JsonProperty("top")]
        public string Top { get; set; } = null;
        [JsonProperty("orderBy")]
        public string OrderBy { get; set; } = null;
    }
}
