using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using CodeMash.Client;
using CodeMash.Repository;
using HrApp;
using HrApp.Domain;
using HrApp.Entities;
using LambdaFunction.Inputs;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaFunction
{
    public class Function
    {
        public async Task<APIGatewayProxyResponse> Handler(CustomEventRequest<CollectionTriggerInput> lambdaEvent)
        {
            var client = new CodeMashClient(HrApp.Settings.ApiKey, HrApp.Settings.ProjectId);
            var taxonomy = new CodeMashTermsService(client);
            var userRecord = JObject.Parse(lambdaEvent.Input.NewRecord);
            var level = JObject.Parse(userRecord["level"].ToString()).ToString();
            var levels = taxonomy.Find<CompetencyLevelMeta>("CompetencyLevelMeta", x => true).List;

            var monitorsRepo = new CodeMashRepository<MonitorEntity>(client);
            var computersRepo = new CodeMashRepository<ComputerEntity>(client);
            var phonesRepo = new CodeMashRepository<PhoneEntity>(client);

            var competencyLevel = levels.First(x => x.Id == level);

            var funds = JArray.Parse(userRecord["cash"].ToString())
                .Select(x => JsonConvert.DeserializeObject<Cash>(x.ToString()))
                .ToList();

            var trips = JArray.Parse(userRecord["business_trips"].ToString())
                .Select(x => JsonConvert.DeserializeObject<Trip>(x.ToString()))
                .ToList();

            trips.ForEach(x => x.MapCountry(client));

            var monitors = JArray.Parse(userRecord["monitors"].ToString())
                .Select(x => x.ToString())
                .ToList();

            var computers = JArray.Parse(userRecord["computers"].ToString())
                .Select(x => x.ToString())
                .ToList();

            var phones = JArray.Parse(userRecord["phones"].ToString()).Select(x => x.ToString()).ToList();

            var allStuff = new EmployeeInfoDto
            {
                Phones = phones.Select(x => phonesRepo.FindOneByIdAsync(x).Result).ToList(),
                Computers = computers.Select(x => computersRepo.FindOneByIdAsync(x).Result).ToList(),
                Monitors = monitors.Select(x => monitorsRepo.FindOneByIdAsync(x).Result).ToList(),
                Trips = trips.ToList(),
                Funds = funds.ToList(),
                CompetencyLevelMeta = competencyLevel.Meta
            };

            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(allStuff),
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}