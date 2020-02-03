using System;
using System.IO;
using System.Collections.Generic;
using HrApp.Entities;
using LambdaFunction.Inputs;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LambdaFunction
{
    [TestFixture]
    public class FunctionTests
    {
        public WishlistEntity FormerWishlist = new WishlistEntity
        {
            ApprovedBy = new List<string>(),
            DeclinedBy = new List<string>(),
            ShouldBeApprovedBy = new List<string>
            {
                "25a5e422-6e07-42d4-9df4-18dae9a88426",
                "353e5ee9-f7f8-47b1-a93e-69b4ddd72c2f",
                "4db186cd-91b9-4538-97b5-40b0f7a139ab"

            },
            Status = "Processing",
            Id = "5e2fe27abebcee0001f9f7d3"
        };
        public WishlistEntity NewWishlist = new WishlistEntity
        {
            ApprovedBy = new List<string>(),
            DeclinedBy = new List<string>(),
            ShouldBeApprovedBy = new List<string>
            {
                "25a5e422-6e07-42d4-9df4-18dae9a88426",
                "353e5ee9-f7f8-47b1-a93e-69b4ddd72c2f",
                "4db186cd-91b9-4538-97b5-40b0f7a139ab"

            },
            Status = "Processing",
            Id = "5e2fe27abebcee0001f9f7d3"
        };

        // Before running these tests, must disable line 31 in Function.cs,
        // because there is a PUT method in that line, that has no effect
        // on the function
        [Test]
        [TestCase("25a5e422-6e07-42d4-9df4-18dae9a88426", ExpectedResult = "Processing")]
        [TestCase("353e5ee9-f7f8-47b1-a93e-69b4ddd72c2f", ExpectedResult = "Processing")]
        [TestCase("4db186cd-91b9-4538-97b5-40b0f7a139ab", ExpectedResult = "Approved")]
        public string WhenSomeoneAgreesTest(string agreedPerson)
        {
            NewWishlist.ApprovedBy.Add(agreedPerson);
            var input = new CustomEventRequest<CollectionTriggerInput>
            {
                Input = new CollectionTriggerInput
                {
                    FormerRecord = JsonConvert.SerializeObject(FormerWishlist),
                    NewRecord = JsonConvert.SerializeObject(NewWishlist)
                },
                ProjectId = "b09eaa56-75eb-42f6-9d77-145ac6f6dedb"
            };

            var function = new Function();

            var result = function.AcceptOrder(input);

            return result.Result.Body;
        }
    }
}