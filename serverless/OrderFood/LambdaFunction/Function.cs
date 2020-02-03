using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using HrApp;
using LambdaFunction.Inputs;
using LambdaFunction.Services;
using LambdaFunction.Settings;
using Newtonsoft.Json.Linq;
using ServiceStack;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaFunction
{
    public class PreferencesDto
    {
        public string Id { get; set; }

        public List<PersonalOrderPreference> Preferences { get; set; }
    }
    
    public class Function
    {
        public async Task<APIGatewayProxyResponse> Handler(CustomEventRequest<BasicInput> lambdaEvent, ILambdaContext context)
        {
            var input = lambdaEvent.Input.Data;
            var data = JsonConvert.DeserializeObject<PreferencesDto>(input);
            
            var repo = new MenuRepository();
            var menu = await repo.GetClosestMenu();
            
            await repo.MakeEmployeeOrder(menu, data.Preferences, new EmployeeEntity { Id = data.Id });


            return new APIGatewayProxyResponse
            {
                Body = lambdaEvent.Input.Data,
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}
