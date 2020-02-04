using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Client;
using CodeMash.Models;
using CodeMash.Repository;
using Newtonsoft.Json;

namespace HrApp.Domain
{
    public class Trip
    {
        [Field("country")]
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        [Field("from")]
        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public float From { get; set; }
        [Field("to")]
        [JsonProperty("to", NullValueHandling = NullValueHandling.Ignore)]
        public float To { get; set; }

        public void MapCountry(CodeMashClient client)
        {
            var taxonomy = new CodeMashTermsService(client);

            var countries = taxonomy.Find<object>("Countries", x => true).List;
            this.Country = countries.First(x => x.Id == Country).Name;
        }
    }
}
