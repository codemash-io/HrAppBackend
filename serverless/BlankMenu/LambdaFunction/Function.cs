using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using HrApp;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaFunction
{
    public class Function
    {
        public async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            var division = new Division { Id = "5d88ae84a792110001fef326"};

            var lunchService = new LunchService
            {
                HrService = new HrService
                {
                    EmployeesRepository = new EmployeesRepository()
                },
                MenuRepository = new MenuRepository()
            };

            var menu = await lunchService.CreateBlankMenu(division);
            var body = new Dictionary<string, string>
            {
                { "menu", menu.Id }
            };

            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(body),
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}
