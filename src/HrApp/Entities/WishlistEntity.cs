using System;
using System.Collections.Generic;
using CodeMash.Models;
using HrApp.Domain;
using Newtonsoft.Json;

namespace HrApp.Entities
{
    [Collection("wishlist")]
    public class WishlistEntity : CustomEntity
    {
        // adding ignore handling, cause json schema doesnt validate null
        [Field("request")]
        [JsonProperty("request", NullValueHandling = NullValueHandling.Ignore)]
        public string Request { get; set; }

        [Field("user")]
        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public string User { get; set; }

        [Field("status")]
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [Field("items")]
        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [Field("order_type")]
        [JsonProperty("order_type", NullValueHandling = NullValueHandling.Ignore)]
        public string OrderType { get; set; }

        [Field("should_be_approved_by")]
        [JsonProperty("should_be_approved_by")]
        public List<string> ShouldBeApprovedBy { get; set; }

        [Field("approved_by")]
        [JsonProperty("approved_by")]
        public List<string> ApprovedBy { get; set; }

        [Field("declined_by")]
        [JsonProperty("declined_by")]
        public List<string> DeclinedBy { get; set; }
    }
}