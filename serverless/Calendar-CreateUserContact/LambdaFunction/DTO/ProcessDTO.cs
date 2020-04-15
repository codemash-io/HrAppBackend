using Microsoft.Graph;
using Newtonsoft.Json;


namespace LambdaFunction
{
    public class ProcessDTO
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("contact")]
        public Contact Contact { get; set; }
    }
}
