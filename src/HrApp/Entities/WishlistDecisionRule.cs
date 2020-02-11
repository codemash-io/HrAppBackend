using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Models;
using Newtonsoft.Json;

namespace HrApp.Entities
{
    [Collection("wishlist-decision-rules")]
    public class WishlistDecisionRule : CustomEntity
    {
        [Field("type")]
        [JsonProperty("type")]
        public string Type { get; set; }
        [Field("roles")]
        [JsonProperty("roles")]
        public List<string> Roles { get; set; }
    }
}
