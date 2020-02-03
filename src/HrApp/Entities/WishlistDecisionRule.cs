using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Models;

namespace HrApp.Entities
{
    [Collection("wishlist-decision-rules")]
    public class WishlistDecisionRule : Entity
    {
        [Field("type")]
        public string OrderType { get; set; }
        [Field("roles")]
        public List<string> Roles { get; set; }
    }
}
