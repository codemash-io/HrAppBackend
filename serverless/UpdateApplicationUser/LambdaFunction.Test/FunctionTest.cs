using System.Linq;
using CodeMash.Client;
using CodeMash.Repository;
using HrApp.Domain;
using HrApp.Entities;
using NUnit.Framework;
using System.IO;
using LambdaFunction.Inputs;
using Moq;

namespace LambdaFunction
{
    [TestFixture]
    public class FunctionTest
    {
        [Test(ExpectedResult = "benas.janusonis@ktu.edu")]
        public string TestFunction()
        {
            var function = new Function();

            var response = function.Handler(new CustomEventRequest<UserTriggerInput>
            {
                Input = new UserTriggerInput
                {
                    NewUser = new InputUser
                    {
                        Email = "benas.janusonis@ktu.edu",
                        Id = "f501ac8d-6229-4fcc-895f-c5f7e22b53a8"
                    }
                }
            });

            return response.Body;
        }
    }
}