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

           Absence items = JsonConvert.DeserializeObject<Absence>(records);

            var hrService = new HrService()
            {
                EmployeesRepository = new EmployeesRepository(),
                NotificationSender = new NotificationSender(),
                AbsenceRequestRepository = new AbsenceRepository()
            };
            string employee = items.EmployeeId;
            string absence = items.AbsenceId;
            //string employee = "5d933d90b7297d000158fc1e";
          //  string absence = "5e6a21600187c000015bb49c";          
            if (Environment.GetEnvironmentVariable("ApiKey") != null)
            {
                HrApp.Settings.ApiKey = Environment.GetEnvironmentVariable("ApiKey");
            }
            await hrService.SendEmailToManagerAboutAbsence(employee, absence);
            // - Response body can be any serializable object
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
