using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using LambdaFunction.Inputs;
using LambdaFunction.Services;
using HrApp;

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
        
        /// <summary>
        /// (Required) Entry method of your Lambda function.
        /// </summary>
        /// <param name="lambdaEvent">Type returned from CodeMash</param>
        /// <param name="context">Context data of a function (function config)</param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> Handler(CustomEventRequest<BasicInput> lambdaEvent, ILambdaContext context)
        {
            string roomName;
            DateTime dateFrom, dateTo;
            if(lambdaEvent.Input.Data != null)
            {
                if (lambdaEvent.Input.Data != null)
                {
                    ProcessDTO items = JsonConvert.DeserializeObject<ProcessDTO>(lambdaEvent.Input.Data);
                    roomName = items.RoomName;
                    dateFrom = items.DateFrom;
                    dateTo = items.DateTo;
                }
                else
                {
                    roomName = Environment.GetEnvironmentVariable("roomName");
                    dateFrom = DateTime.Parse(Environment.GetEnvironmentVariable("dateFrom"));
                    dateTo = DateTime.Parse(Environment.GetEnvironmentVariable("dateTo"));
                }
            }
            else
            {
                roomName = Environment.GetEnvironmentVariable("roomName");
                dateFrom = DateTime.Parse(Environment.GetEnvironmentVariable("dateFrom"));
                dateTo = DateTime.Parse(Environment.GetEnvironmentVariable("dateTo"));
            }
            if (Environment.GetEnvironmentVariable("ApiKey") != null)
            {
                HrApp.Settings.ApiKey = Environment.GetEnvironmentVariable("ApiKey");
            }
            else
                throw new BusinessException("ApiKey not set");
            if (string.IsNullOrEmpty(roomName))
                throw new BusinessException("All fields must be filled with data");

            var repo = new GraphRepository();

            var roomEvents = await repo.GetCalendarEventsByDate(roomName, dateFrom, dateTo);

            // - Response body can be any serializable object
            var response = new
            {
                roomName,
                dateFrom,
                dateTo,
                roomEvents,
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
