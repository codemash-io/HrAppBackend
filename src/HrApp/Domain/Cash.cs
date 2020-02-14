using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HrApp.Domain
{
    public class Cash
    {
        [Field("amount")]
        [JsonProperty("amount")]
        public Price Amount { get; set; }

        [Field("date")]
        [JsonProperty("date")]
        public float Date { get; set; }

        [Field("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        public CashDTO GetCashDto()
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var dateTime = startTime.AddMilliseconds(Date);

            return new CashDTO
            {
                Amount = this.Amount,
                Date = $"{dateTime.Year}-{dateTime.Month:D2}-{dateTime.Day:D2}",
                Description = this.Description
            };
        }
    }
}
