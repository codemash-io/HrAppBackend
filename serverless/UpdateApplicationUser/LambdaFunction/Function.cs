using System;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using CodeMash.Repository;
using HrApp;
using HrApp.Entities;
using LambdaFunction.Inputs;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaFunction
{
    public class Function
    {
        public APIGatewayProxyResponse Handler(CustomEventRequest<UserTriggerInput> lambdaEvent)
        {
            var repo = new CodeMashRepository<UserDbEntity>(HrApp.Settings.Client);

            var email = lambdaEvent.Input.NewUser.Email;
            var userId = lambdaEvent.Input.NewUser.Id;

            var employeeToUpdate = repo.FindOne(x => x.BusinessEmail == email);
            var updateDefinition = Builders<UserDbEntity>.Update.Set(x => x.application_User, userId);

            repo.UpdateOne(employeeToUpdate.Id, updateDefinition, new DatabaseUpdateOneOptions());

            return new APIGatewayProxyResponse
            {
                Body = email,
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
            };
        }
    }
}