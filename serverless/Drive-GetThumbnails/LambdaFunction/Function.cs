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
using Microsoft.Graph;

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
        public async Task<List<ThumbnailSet>> Handler(CustomEventRequest<BasicInput> lambdaEvent, ILambdaContext context)
        {
            string type, typeId, itemId, select;
            if (lambdaEvent.Input.Data != null)
            {
                ProcessDTO items = JsonConvert.DeserializeObject<ProcessDTO>(lambdaEvent.Input.Data);
                if (items.APiKey != null)
                    HrApp.Settings.ApiKey = items.APiKey;
                type = items.Type;
                typeId = items.TypeId;
                itemId = items.ItemId;
                select = items.Select;
            }
            else
            {
                type = Environment.GetEnvironmentVariable("type");
                typeId = Environment.GetEnvironmentVariable("typeId");
                itemId = Environment.GetEnvironmentVariable("itemId");
                select = Environment.GetEnvironmentVariable("select");
                if (Environment.GetEnvironmentVariable("apiKey") != null)
                    HrApp.Settings.ApiKey = Environment.GetEnvironmentVariable("apiKey");
            }

            if (HrApp.Settings.ApiKey == null)
                throw new BusinessException("ApiKey not set");
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(itemId))
                throw new BusinessException("Type and itemId is required!");

            IGraphFileRepository graphFileRepo = new GraphFileRepository();
            var thumbs = await graphFileRepo.GetThumbnails(type, itemId, typeId, select);

            return thumbs;
        }
    }
}
