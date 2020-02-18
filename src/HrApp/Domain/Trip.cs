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
        public string Country { get; set; }
        [Field("from")]
        [JsonProperty("from")]
        public float From { get; set; }
        [Field("to")]
        [JsonProperty("to")]
        public float To { get; set; }

        public void MapCountry()
        {
            var taxonomy = new CodeMashTermsService(HrApp.Settings.Client);

            var countries = taxonomy.Find<object>("Countries", x => true).List;
            this.Country = countries.First(x => x.Id == Country).Name;
        }

        public TripDTO GeTripDto()
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var fromDateTime = startTime.AddMilliseconds(From);//+ TimeSpan.FromTicks((long)From);
            var toDateTime = startTime.AddMilliseconds(To);// + TimeSpan.FromTicks((long)To);

            return new TripDTO
            {
                Country = this.Country,
                From = $"{fromDateTime.Year}-{fromDateTime.Month:D2}-{fromDateTime.Day:D2}",
                To = $"{toDateTime.Year}-{toDateTime.Month:D2}-{toDateTime.Day:D2}"
            };
        }
    }
}
