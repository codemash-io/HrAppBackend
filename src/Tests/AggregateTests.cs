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
            var notificationSender = new NotificationSender();
            var emails = new List<string> {"mantasdaunoravicius@gmail.com" };

            //+20h because cm deserializes date in utc so i add 20 to make potential end of the day 
             //and to make sure that aggregate return correct day results
            var today = new DateTime(2020,06,30).AddHours(20);

            int quarterNumber = (today.Month-1)/3+1;
            DateTime firstDayOfQuarter = new DateTime(today.Year, (quarterNumber - 1) * 3 + 1, 1).AddHours(20);
            DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1).AddHours(20);

            if (today.Day == DateTime.DaysInMonth(today.Year, today.Month))
            {
                //monthly
                var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
                var summary = await aggregateRepo.GetWhishListSummary(firstDayOfMonth, today);
                if(summary.Count == 0)
                    summary.Add(new WishlistSummary{Type="No wishes during this period", Total=""});
                await notificationSender.SendWishlistSummaryEmail(firstDayOfMonth, today, emails, summary);
            }
            if(today.Date == lastDayOfQuarter.Date)
            {
                //every quarter
                var summary = await aggregateRepo.GetWhishListSummary(firstDayOfQuarter, lastDayOfQuarter);
                if(summary.Count == 0)
                    summary.Add(new WishlistSummary{Type="No wishes during this period", Total=""});
                await notificationSender.SendWishlistSummaryEmail(firstDayOfQuarter, lastDayOfQuarter, emails, summary);
            }


           /* var summary = await aggregateRepo.GetWhishListSummary(from, to);
            if(summary.Count == 0)
                summary.Add(new WishlistSummary{Type="No wishes during this period", Total=""});

            await notificationSender.SendWishlistSummaryEmail(from, to, emails, summary);*/
            //Assert.IsNotEmpty(summary);
        }

    }
}