using Newtonsoft.Json;

namespace HrApp
{
    public class Office365User
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("displayName")]
        public string FullName { get; set; }
        [JsonProperty("givenName")]
        public string FirstName { get; set; }
        [JsonProperty("surname")]
        public string LastName { get; set; }
        [JsonProperty("mail")]
        public string Email { get; set; }
        [JsonProperty("mobilePhone")]
        public string PhoneNumber { get; set; }
        [JsonProperty("userPrincipalName")]
        public string UserPrincipalName { get; set; }
    }
}