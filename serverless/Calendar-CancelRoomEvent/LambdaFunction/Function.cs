using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using LambdaFunction.Inputs;
using LambdaFunction.Services;
using LambdaFunction.Settings;
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
        public async Task<Dictionary<string, bool>> Handler(CustomEventRequest<BasicInput> lambdaEvent, ILambdaContext context)
        {
            string roomName, eventId;
            if (lambdaEvent.Input.Data != "")
            {
                ProcessDTO items = JsonConvert.DeserializeObject<ProcessDTO>(lambdaEvent.Input.Data);
                roomName = items.RoomName;
                eventId = items.EventId;
                if (items.ApiKey != null)
                    HrApp.Settings.ApiKey = items.ApiKey;
            }
            else
            {
                roomName = Environment.GetEnvironmentVariable("roomName");
                eventId = Environment.GetEnvironmentVariable("eventId");
                if (Environment.GetEnvironmentVariable("apiKey") != null)
                    HrApp.Settings.ApiKey = Environment.GetEnvironmentVariable("apiKey");
            }
            if (HrApp.Settings.ApiKey == null)
                throw new BusinessException("ApiKey not set");
            if (string.IsNullOrEmpty(roomName) || string.IsNullOrEmpty(eventId))
                throw new BusinessException("All fields must be filled with data");

            var roomService = new RoomBookerService()
            {
                GraphRepository = new GraphRepository(),
                GraphEventRepository = new GraphEventsRepository(),
                GraphUserRepository = new GraphUserRepository()
            };
            var isCanceled = await roomService.CancelBooking(eventId, roomName);

            var response = new Dictionary<string, bool>
            {
                { "isCanceled", isCanceled }
            };
            return response;
        }
    }
}
