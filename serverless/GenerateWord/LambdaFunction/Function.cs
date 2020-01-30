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
        
        // (Required if adding other constructors. Otherwise, optional.) A default constructor
        // called by Lambda. If you are adding your custom constructors,
        // default constructor with no parameters must be added
        public Function() : this (new ExampleService()) {}

        // (Optional) An example of injecting a service. As a default constructor is called by Lambda
        // this constructor has to be called from default constructor
        public Function(IExampleService exampleService)
        {
            _exampleService = exampleService;
        }
       
        private static CodeMashClient Client => new CodeMashClient(HrApp.Settings.ApiKey, HrApp.Settings.ProjectId);
        /// <summary>
        /// (Required) Entry method of your Lambda function.
        /// </summary>
        /// <param name="lambdaEvent">Type returned from CodeMash</param>
        /// <param name="context">Context data of a function (function config)</param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> Handler(CustomEventRequest<BasicInput> lambdaEvent, ILambdaContext context)
        {
            // - Get environment variable
            var employeeid = Environment.GetEnvironmentVariable("employee");
            var datefrom = DateTime.Parse(Environment.GetEnvironmentVariable("dateFrom"));
            var dateTo = DateTime.Parse(Environment.GetEnvironmentVariable("dateTo"));
            var type = Environment.GetEnvironmentVariable("type");
            var text = Environment.GetEnvironmentVariable("text");

            var service = new CodeMashRepository<EmployeeEntity>(Client);

            var person = await service.FindOneByIdAsync(
                    employeeid,
                    new DatabaseFindOneOptions()
                );

            var word = new WordCreator()
            {
                FileRepo = new FileRepository()
            };
            await word.GenerateWordAsync(datefrom, dateTo, person, text);
                                 

            var response = new
            {
                employeeid, 
                datefrom,
                dateTo,
                type,
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
