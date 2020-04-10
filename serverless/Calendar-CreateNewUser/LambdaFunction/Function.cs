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
using GraphUser = Microsoft.Graph.User;
using System.Linq;
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
        public async Task<APIGatewayProxyResponse> Handler(CustomEventRequest<BasicInput> lambdaEvent, ILambdaContext context)
        {
            string displayName, password, email;
            if (lambdaEvent.Input.Data != null)
            {
                if (lambdaEvent.Input.Data != null)
                {
                    ProcessDTO items = JsonConvert.DeserializeObject<ProcessDTO>(lambdaEvent.Input.Data);
                    displayName = items.DisplayName;
                    email = items.Email;
                    password = items.Password;
                }
                else
                {
                    displayName = Environment.GetEnvironmentVariable("displayName");
                    email = Environment.GetEnvironmentVariable("email");
                    password = Environment.GetEnvironmentVariable("password");
                }
            }
            else
            {
                displayName = Environment.GetEnvironmentVariable("displayName");
                email = Environment.GetEnvironmentVariable("email");
                password = Environment.GetEnvironmentVariable("password"); ;
            }
            if (Environment.GetEnvironmentVariable("ApiKey") != null)
            {
                HrApp.Settings.ApiKey = Environment.GetEnvironmentVariable("ApiKey");
            }
            else
                throw new BusinessException("ApiKey not set");
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(displayName) || string.IsNullOrEmpty(password))
                throw new BusinessException("All fields must be filled with data");

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                    throw new BusinessException("Your email is invalid");
            }
            catch
            {
                throw new BusinessException("Your email is invalid");
            }
            if (password.Length < 8)
                throw new BusinessException("Password must contain at least 8 characters");
            if (!password.Any(char.IsUpper))
                throw new BusinessException("Password must contain at least one upper char");
            if (!password.Any(char.IsDigit))
                throw new BusinessException("Password must contain at least one digit");

            var user = new GraphUser
            {
                AccountEnabled = true,
                DisplayName = displayName,
                UserPrincipalName = email,
                MailNickname = email.Split('@')[0],
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = true,
                    Password = password
                }
            };
            var graphUserRepo = new GraphUserRepository();

            var createdUser = await graphUserRepo.CreateGraphUser(user);

            // - Get variable from appsettings.json file
            var nestedSettings = AppSettings.GetString("NestedParams:NestedParam1");

            // - Response body can be any serializable object
            var response = new
            {
                createdUser,
                nestedSettings, 
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
