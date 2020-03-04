using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using CodeMash.Client;
using CodeMash.Membership.Services;
using CodeMash.Models;
using CodeMash.Notifications.Push.Services;
using CodeMash.Repository;
using HrApp;
using Isidos.CodeMash.ServiceContracts;
using LambdaFunction.Inputs;
using LambdaFunction.Services;
using LambdaFunction.Settings;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using ServiceStack;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaFunction
{
    [Collection("absence-requests")]
    public class AbsenceRequest : CustomEntity
    {
        [Field("employee")] public string Employee { get; set; }
        [Field("status")] public string Status { get; set; }
        [Field("should_be_approved_by")] public List<string> ShouldBeApprovedBy { get; set; }
    }

    [Collection("pc-employees")]
    public class PCEmployee : Entity
    {
        [Field("first_name")]
        public string FirstName { get; set; }
        [Field("last_name")]
        public string LastName { get; set; }
        [Field("manager")]
        [JsonProperty("manager", NullValueHandling = NullValueHandling.Ignore)]
        public string Manager { get; set; }
        [Field("application_user")]
        [JsonProperty("application_user", NullValueHandling = NullValueHandling.Ignore)]
        public string AppUserID { get; set; }
    }

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
        public async Task<APIGatewayProxyResponse> Handler(CustomEventRequest<CollectionTriggerInput> lambdaEvent, ILambdaContext context)
        {
            var response = JsonConvert.DeserializeObject<AbsenceRequest>(lambdaEvent.Input.NewRecord);
            var requestId = response.Id;

            #region Initialization

            const string appSettings = "appsettings.json";
            string projectIdString;
            string apiKey;

            using (StreamReader reader = new StreamReader(appSettings))
            {
                string jsonString = reader.ReadToEnd();
                var dataJson = JObject.Parse(jsonString);
                apiKey = dataJson["CodeMash"]["ApiKey"].ToString();
                projectIdString = dataJson["CodeMash"]["ProjectId"].ToString();
            }

            var projectId = Guid.Parse(projectIdString);

            // 2. Create a general client for API calls
            var client = new CodeMashClient(apiKey, projectId);
            var pushService = new CodeMashPushService(client);

            // 3. Create a service object
            List<string> usersThatShouldApprove = new List<string>();
            List<string> requiredRoles = new List<string> { "ceo", "accountant", "cto", "hr" };

            var employees = new CodeMashRepository<PCEmployee>(client);
            var requests = new CodeMashRepository<AbsenceRequest>(client);

            #endregion

            var membershipService = new CodeMashMembershipService(client);

            var allUsersList = membershipService.GetUsersList(new GetUsersRequest());

            // Employee ID
            var employeeId = requests.FindOne(x => x.Id == requestId).Employee;
            // Employee manager ID
            var employeeManagerId = employees.FindOne(x => x.Id == employeeId).Manager;
            var employeeManagerUserId = employees.FindOne(x => x.Id == employeeManagerId).AppUserID;

            // Adding user with required roles to local users that should approve list
            foreach (var user in allUsersList.Result)
            {
                foreach (var userRole in user.Roles)
                {
                    if (requiredRoles.Contains(userRole.Name))
                    {
                        usersThatShouldApprove.Add(user.Id);
                        break;
                    }
                }
            }

            // Adding manager to users that should approve list
            foreach (var user in allUsersList.Result)
            {
                if (user.Id == employeeManagerUserId && !usersThatShouldApprove.Contains(user.Id))
                {
                    Console.WriteLine(user.FirstName + " " + user.LastName);
                    usersThatShouldApprove.Add(user.Id);
                    break;
                }
            }

            // Adding to should be approved by list
            foreach (var userId in usersThatShouldApprove)
            {
                requests.UpdateOne(x => x.Id == requestId,
                                   Builders<AbsenceRequest>.Update.AddToSet("should_be_approved_by", userId));
            }

            List<Guid> userGuids = new List<Guid>();
            string templateId = "02dd0ea0-79b2-4a9c-a10b-4009894958b3";

            // Convertion to guid list
            foreach (var userId in usersThatShouldApprove)
            {
                userGuids.Add(Guid.Parse(userId));
            }

            // Sending notifications to each user
            pushService.SendPushNotification(new SendPushNotificationRequest
                {
                    TemplateId = Guid.Parse(templateId),
                    Users = userGuids
                }
            );

            return new APIGatewayProxyResponse
            {
                Body = "lol",
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}
