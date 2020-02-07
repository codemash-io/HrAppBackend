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

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaFunction
{

    public class Function
    {
        private static CodeMashClient Client => new CodeMashClient(HrApp.Settings.ApiKey, HrApp.Settings.ProjectId);
        public async Task<APIGatewayProxyResponse> Handler(CustomEventRequest<BasicInput> lambdaEvent, ILambdaContext context)
        {
            //var lunchDate = DateTime.Parse(Environment.GetEnvironmentVariable("lunchTime"));
           // var menuID = Environment.GetEnvironmentVariable("menuID");


            var lunchDate = DateTime.Parse("2020-02-09");
            var menuID = "5e3c016f214efe00018721ab";


            var service = new CodeMashRepository<MenuEntity>(Client);
            var menuEntity = await service.FindOneByIdAsync(
                    menuID,
                    new DatabaseFindOneOptions()
                );
            var menu = new Menu(DateTime.Now, null, null)
            {
                Id = menuID,
                Division = new Division() { Id = menuEntity.DivisionId}
            
            };

            var lunchService = new LunchService() {
                HrService = new HrService() { EmployeesRepository = new EmployeesRepository()},
                MenuRepository = new MenuRepository(),
                NotificationSender = new NotificationSender() };


            await lunchService.AdjustMenuLunchTime(lunchDate, menu, menuEntity.Employees);
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
