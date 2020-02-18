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
        #region inputJson

        public static string input =
            "{\r\n  \"request\": \"coffe mug\",\r\n  \"user\": \"f501ac8d-6229-4fcc-895f-c5f7e22b53a8\",\r\n  \"status\": \"Declined\",\r\n  \"items\": [\r\n    {\r\n      \"price\": {\r\n        \"value\": 25,\r\n        \"currency\": \"EUR\"\r\n      },\r\n      \"web_link\": \"http://www.google.com\",\r\n      \"description\": \"a plastic coffee cup convenient for travel\"\r\n    }\r\n  ],\r\n  \"order_type\": \"5e2fdeeabebcee0001f878f4\",\r\n  \"should_be_approved_by\": [\r\n    \"f501ac8d-6229-4fcc-895f-c5f7e22b53a8\"\r\n  ],\r\n  \"approved_by\": [],\r\n  \"declined_by\": [\r\n    \"f501ac8d-6229-4fcc-895f-c5f7e22b53a8\"\r\n  ],\r\n  \"_id\": \"5e41c583cd5588000143185f\",\r\n  \"_meta\": {\r\n    \"responsibleUserId\": \"a03962f5-1f9e-4466-a36d-f45ede7239cb\",\r\n    \"createdOn\": 1581368944841,\r\n    \"modifiedOn\": 1581377488123\r\n  }\r\n}";
        #endregion
        public static WishlistEntity FormerWishlist = new WishlistEntity
        {
            Status = "Pending",
            Id = "5e41c583cd5588000143185f"
        };

        public static WishlistEntity NewWishlist = new WishlistEntity();

        public CustomEventRequest<CollectionTriggerInput> LambdaInput = new CustomEventRequest<CollectionTriggerInput>
        {
            Input = new CollectionTriggerInput
            {
                FormerRecord = JsonConvert.SerializeObject(FormerWishlist),
            },
            ProjectId = "b09eaa56-75eb-42f6-9d77-145ac6f6dedb"
        };
        
        //before running these tests, disable (comment) line 46 in Function.cs, in line 46,
        //there is a PUT method, therefore that line has no impact on the logic of the method
        //Expected results are all the same, because the
        //Users DB is not filled with users, who have more roles
        [Test]
        //Order type - SDK
        [TestCase("5e2aa8f52fe37a00015a2b07", ExpectedResult = "[\"f501ac8d-6229-4fcc-895f-c5f7e22b53a8\",\"353e5ee9-f7f8-47b1-a93e-69b4ddd72c2f\"]")]
        //Order type - Office small items
        [TestCase("5e2aa8d12fe37a00015a2b06", ExpectedResult = "[\"f501ac8d-6229-4fcc-895f-c5f7e22b53a8\"]")]
        //Order type - Miscellaneous
        [TestCase("5e2aa90a2fe37a00015a2b09", ExpectedResult = "[\"f501ac8d-6229-4fcc-895f-c5f7e22b53a8\"]")]
        //Order type - Software license
        [TestCase("5e2aa8fc2fe37a00015a2b08", ExpectedResult = "[\"353e5ee9-f7f8-47b1-a93e-69b4ddd72c2f\"]")]
        public string GetUsersByOrderTypeTest(string orderType)
        {
            NewWishlist = JsonConvert.DeserializeObject<WishlistEntity>(input);
            NewWishlist.OrderType = orderType;
            LambdaInput.Input.NewRecord = JsonConvert.SerializeObject(NewWishlist);

            var function = new Function();

            var result = function.Handler(LambdaInput);

            return result.Body;
        }
    }
}
