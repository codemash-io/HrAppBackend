using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HrApp.Entities
{
    public class CustomEntity : IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
        [Field("_id")]
        public string Id { get; set; }
    }
}
