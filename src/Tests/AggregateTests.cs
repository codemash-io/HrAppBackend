using HrApp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class AggregateTests
    {

        IAggregateRepository aggregateRepo;
        [SetUp]
        public void Setup()
        {
            aggregateRepo = new AggregateRepository();
        }

        [Test]
        public async Task Process_Noob_Form_test()
        {
            await aggregateRepo.LunchMenuReport("5e6a1b980187c000015b0767");
        }


        [Test]
        public async Task GetWishListSummary()
        {
            var from = new DateTime(2020, 02, 01, 0, 0, 0, DateTimeKind.Utc);
            var to = new DateTime(2020, 05, 01, 0, 0, 0, DateTimeKind.Utc);
            var summary = await aggregateRepo.GetWhishListSummary(from, to);
            Assert.IsNotEmpty(summary);
        }

        [Test]
        public async Task SendWishlistSummaryEmail()
        {
            var repo = new NotificationSender();
            var from = new DateTime(2020, 02, 01, 0, 0, 0, DateTimeKind.Utc);
            var to = new DateTime(2020, 05, 01, 0, 0, 0, DateTimeKind.Utc);
            var email = "mantasdaunoravicius@gmail.com";
            var summary = await aggregateRepo.GetWhishListSummary(from, to);


            await repo.SendWishlistSummaryEmail(from, to, email, summary);
            Assert.IsNotEmpty(summary);
        }

    }
}