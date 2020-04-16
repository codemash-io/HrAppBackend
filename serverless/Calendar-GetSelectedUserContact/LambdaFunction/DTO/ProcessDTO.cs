using Newtonsoft.Json;

namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("contactId")]
        public string ContactId { get; set; }
        [JsonProperty("expand")]
        public string Expand { get; set; }
        [JsonProperty("select")]
        public string Select { get; set; }
    }
}
