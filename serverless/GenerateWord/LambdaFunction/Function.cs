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

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaFunction
{
    public class Function
    {
        private readonly IExampleService _exampleService;

        public class AbsenceClass
        {
           
            [JsonProperty(PropertyName = "employee")]
            public string Employee { get; set; }
            [JsonProperty(PropertyName = "_id")]
            public string Absence { get; set; }


        }
        private static CodeMashClient Client => new CodeMashClient(HrApp.Settings.ApiKey, HrApp.Settings.ProjectId);
        /// <summary>
        /// (Required) Entry method of your Lambda function.
        /// </summary>
        /// <param name="lambdaEvent">Type returned from CodeMash</param>
        /// <param name="context">Context data of a function (function config)</param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> Handler(CustomEventRequest<CollectionTriggerInput> lambdaEvent, ILambdaContext context)
        {
            var reason = Environment.GetEnvironmentVariable("reason");
            var type = Environment.GetEnvironmentVariable("type");
            var end = Environment.GetEnvironmentVariable("end");
            var absencetype = Environment.GetEnvironmentVariable("absencetype");
            var days = Environment.GetEnvironmentVariable("days");
            var records = lambdaEvent.Input.NewRecord;

            AbsenceClass items = JsonConvert.DeserializeObject<AbsenceClass>(records);


            var service = new CodeMashRepository<EmployeeEntity>(Client);
            var person = await service.FindOneByIdAsync(
                    items.Employee,
                    new DatabaseFindOneOptions()
                );

            var serviceAbsence = new CodeMashRepository<AbsenceRequestEntity>(Client);
            var absenceRequesInfo = await serviceAbsence.FindOneByIdAsync(
                    items.Absence,
                    new DatabaseFindOneOptions()
                );

            var word = new WordCreator()
            {
                FileRepo = new FileRepository()
            };
            await word.GenerateWordAsync(person, reason, absenceRequesInfo, end, absencetype, days);
                       
            var response = new
            {
                person,                
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
