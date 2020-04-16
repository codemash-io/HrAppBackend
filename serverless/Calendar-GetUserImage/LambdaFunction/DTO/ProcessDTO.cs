using Newtonsoft.Json;

namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("imageSize")]
        public string ImageSize { get; set; }
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
    }
}
