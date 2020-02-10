using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using LambdaFunction.Inputs;
using LambdaFunction.Services;
using LambdaFunction.Settings;
using CodeMash.Client;
using HrApp;
using CodeMash.Repository;
using MongoDB.Driver;
using System.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaFunction
{
    public class MenuClass
    {
        [JsonProperty(PropertyName = "employees")]
        public List<string> Employee { get; set; }

        [JsonProperty(PropertyName = "planned_lunch_date")]
        public float LunchTime { get; set; }
    }

    public class Function
    {
        private static CodeMashClient Client => new CodeMashClient(HrApp.Settings.ApiKey, HrApp.Settings.ProjectId);
        public async Task<APIGatewayProxyResponse> Handler(CustomEventRequest<CollectionTriggerInput> lambdaEvent, ILambdaContext context)
        {
            var records = lambdaEvent.Input.NewRecord;

            MenuClass items = JsonConvert.DeserializeObject<MenuClass>(records);

            var notificationSender = new NotificationSender();
            var employeeRepo = new EmployeesRepository();
                        
            var employeesRepo = new CodeMashRepository<EmployeeEntity>(Client);
            var filter = Builders<EmployeeEntity>.Filter.In(x => x.Id, items.Employee.Distinct());
            var projection = Builders<EmployeeEntity>.Projection.Include(x => x.UserId);
            var employees = await employeesRepo.FindAsync<EmployeeEntity>(filter, projection);

            var employeesGuids = employees.Items
                                .Where(x => x.UserId != Guid.Empty)
                                .Select(x => x.UserId)
                                .ToList();

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);           
            var dtDateTime = epoch.AddMilliseconds(items.LunchTime).ToLocalTime();

            await notificationSender.SendNotificationAboutLunchDateChanges(employeesGuids, dtDateTime);

            var response = new
            {
                dtDateTime,
                employees,
                employeesGuids,
                lambdaEvent,
            };

            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(response),
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}
