using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using CodeMash.Client;
using HrApp.Entities;
using Isidos.CodeMash.ServiceContracts;
using LambdaFunction.Inputs;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;
using File = System.IO.File;

namespace LambdaFunction
{
    [TestFixture]
    class FunctionTests
    {
        public static WishlistEntity FormerWishlist = new WishlistEntity
        {
            Status = "Pending",
            Id = "5e343c37bfc8db00011eb3e8"
        };

        public static WishlistEntity NewWishlist = new WishlistEntity
        {
            OrderType = "5e2aa8f52fe37a00015a2b07",
            Status = "Processing",
        };

        public CustomEventRequest<CollectionTriggerInput> LambdaInput = new CustomEventRequest<CollectionTriggerInput>
        {
            Input = new CollectionTriggerInput
            {
                FormerRecord = JsonConvert.SerializeObject(FormerWishlist),
            },
            ProjectId = "b09eaa56-75eb-42f6-9d77-145ac6f6dedb"
        };
        
        //before running these tests, disable (comment) line 49 in Function.cs,
        //in line 49, there is a PUT method, therefore that line has no impact on the unit tests
        //Expected results are all the same, because the Users DB is not filled with
        //users, who have more roles
        [Test]
        //Order type - SDK
        [TestCase("5e2aa8f52fe37a00015a2b07", ExpectedResult = "[\"353e5ee9-f7f8-47b1-a93e-69b4ddd72c2f\"]")]
        //Order type - Office small items
        [TestCase("5e2aa8d12fe37a00015a2b06", ExpectedResult = "[\"353e5ee9-f7f8-47b1-a93e-69b4ddd72c2f\"]")]
        //Order type - Miscellaneous
        [TestCase("5e2aa90a2fe37a00015a2b09", ExpectedResult = "[\"353e5ee9-f7f8-47b1-a93e-69b4ddd72c2f\"]")]
        //Order type - Software license
        [TestCase("5e2aa8fc2fe37a00015a2b08", ExpectedResult = "[\"353e5ee9-f7f8-47b1-a93e-69b4ddd72c2f\"]")]
        public string GetUsersByOrderTypeTest(string orderType)
        {
            Environment.SetEnvironmentVariable("ApiKey", "96WLxsvp7FNolruRNIMYycgVT7rI4_Et");
            NewWishlist.OrderType = orderType;
            LambdaInput.Input.NewRecord = JsonConvert.SerializeObject(NewWishlist);

            var function = new Function();

            var result = function.Handler(LambdaInput);

            return result.Body;
        }
    }
}
