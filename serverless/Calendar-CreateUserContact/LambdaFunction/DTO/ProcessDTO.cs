using Microsoft.Graph;
using Newtonsoft.Json;


namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("contact")]
        public Contact Contact { get; set; }
    }
}
