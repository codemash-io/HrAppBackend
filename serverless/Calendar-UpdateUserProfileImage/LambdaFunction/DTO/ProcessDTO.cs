using Newtonsoft.Json;

namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("image")]
        public byte[] ImageBytes { get; set; }
    }
}
