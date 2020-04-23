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
        [JsonProperty("thumbnailId")]
        public string ThumbnailId { get; set; }
        [JsonProperty("size")]
        public string Size { get; set; }

    }
}
