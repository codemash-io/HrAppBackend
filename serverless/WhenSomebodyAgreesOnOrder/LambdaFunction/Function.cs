using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using CodeMash.Membership.Services;
using CodeMash.Repository;
using LambdaFunction.Inputs;
using HrApp.Entities;
using Isidos.CodeMash.ServiceContracts;
using MongoDB.Driver;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaFunction
{
    public class Function
    {
        public async Task<APIGatewayProxyResponse> AcceptOrder(CustomEventRequest<CollectionTriggerInput> lambdaEvent)
        {
            var oldRecord = JsonConvert.DeserializeObject<WishlistEntity>(lambdaEvent.Input.FormerRecord);
            var newRecord = JsonConvert.DeserializeObject<WishlistEntity>(lambdaEvent.Input.NewRecord);
            var wishlistRepo = new CodeMashRepository<WishlistEntity>(HrApp.Settings.Client);

            if (oldRecord.ApprovedBy.Count < newRecord.ApprovedBy.Count)
            {
                if (newRecord.ApprovedBy.Count == newRecord.ShouldBeApprovedBy.Count)
                {
                    newRecord.Status = "Approved";
                    await wishlistRepo.ReplaceOneAsync(x => x.Id == newRecord.Id, newRecord);
                }
            }

            return new APIGatewayProxyResponse
            {
                Body = newRecord.Status,
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}