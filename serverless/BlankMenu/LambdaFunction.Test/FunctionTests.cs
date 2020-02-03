using System;
using System.Collections.Generic;
using System.Text;
using CodeMash.Repository;
using HrApp.Entities;
using LambdaFunction.Inputs;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using NUnit.Framework;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace LambdaFunction
{
    [TestFixture]
    class FunctionTests
    {
        [Test]
        [TestCase("353e5ee9-f7f8-47b1-a93e-69b4ddd72c2f", ExpectedResult = "Processing")]
        [TestCase("a163e81d-811c-4e15-afd1-f761304bb4f3", ExpectedResult = "Processing")]
        [TestCase("25a5e422-6e07-42d4-9df4-18dae9a88426", ExpectedResult = "Accepted")]
        public string AcceptOrderTest(string initiatedUserId)
        {
            var wishlistRepo = new CodeMashRepository<WishlistEntity>(HrApp.Settings.cClient);
            var wishlist = wishlistRepo.FindOneById("5e340767f06da80001fb115e");
            var newWishlist = wishlist;


            var function = new Function();
            var lambdaEvent = new CustomEventRequest<CollectionTriggerInput>
            {
                Input = new CollectionTriggerInput
                {
                    FormerRecord = JsonConvert.SerializeObject(wishlist),
                    NewRecord = JsonConvert.SerializeObject(newWishlist),
                    InitiatorUserId = initiatedUserId
                }
            };

            var response = function.AcceptOrder(lambdaEvent);
            return response.Result.Body;
        }
    }
}
