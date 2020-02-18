using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using CodeMash.Models;
using HrApp.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ServiceStack;

namespace HrApp.Entities
{
    public class CompetencyLevelMeta : Entity
    {
        [JsonProperty("price")]
        public Price Price { get; set; }
        [JsonProperty("net_price")]
        public Price NetPrice { get; set; }
        [JsonProperty("training")]
        public Price Training { get; set; }
        [JsonProperty("budgetfund")]
        public Price BudgetFund { get; set; }
    }
}