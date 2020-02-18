using System.Collections.Generic;
using CodeMash.Models;
using HrApp.Domain;
using Newtonsoft.Json;

namespace HrApp.Entities
{
    //application_user, business_trips and level is named not in PascalCase, because
    //for some reason PascalCase naming has problems with taxonomies
    [Collection("pc-employees")]
    public class UserDbEntity : Entity
    {
        [Field("level")]
        [JsonProperty("level")]
        public string level { get; set; }

        [Field("phones")]
        [JsonProperty("phones")]
        public List<string> Phones { get; set; }

        [Field("computers")]
        [JsonProperty("computers")]
        public List<string> Computers { get; set; }

        [Field("monitors")]
        [JsonProperty("monitors")]
        public List<string> Monitors { get; set; }

        [Field("cash")]
        [JsonProperty("cash")]
        public List<Cash> Cash { get; set; }

        [Field("trainings")]
        [JsonProperty("trainings")]
        public List<Cash> Trainings { get; set; }

        [Field("business_trips")]
        [JsonProperty("business_trips")]
        public List<Trip> business_trips { get; set; }

        [Field("application_user")]
        public string ApplicationUser { get; set; }
    }
}