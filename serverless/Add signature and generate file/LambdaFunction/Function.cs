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
     
        public async Task<APIGatewayProxyResponse> Handler(CustomEventRequest<CollectionTriggerInput> lambdaEvent, ILambdaContext context)
        {
            var records = lambdaEvent.Input.NewRecord;

            DTO items = JsonConvert.DeserializeObject<DTO>(records);
            if (Environment.GetEnvironmentVariable("ApiKey") != null)
            {
                HrApp.Settings.ApiKey = Environment.GetEnvironmentVariable("ApiKey");
            }
            var ImportFileRepo = new ImportFileRepository();

            var hrService = new HrService()
            {
                FileRepository = new FileRepository(),
                NoobRepository = new NoobRepository(),
                EmployeesRepository = new EmployeesRepository(),
                AbsenceRequestRepository = new AbsenceRepository(),
                NotificationSender = new NotificationSender()
            };

              await hrService.GenerateFileWithSignatureAndInsert(items.AbsenceRequestId);

            var response = new
            {
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
