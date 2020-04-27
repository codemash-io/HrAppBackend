using CodeMash.Client;
using CodeMash.Repository;
using HrApp.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public class AggregateRepository:IAggregateRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);
        public async Task<LunchOrderAggregate> LunchMenuReport(string lunchMenuId)
        {
            var service = new CodeMashRepository<MenuEntity>(Client);
            var menuInfo = await service.AggregateAsync<LunchOrderAggregate>(Guid.Parse("fd9cd4e7-f133-49d2-b57d-d45e8f901884"), new AggregateOptions
            {
                Tokens = new Dictionary<string, string>()
                {
                       { "id", lunchMenuId },
                }
            });

            return menuInfo[0];
        }

        public async Task<PersonalOrdersAggregate> LunchMenuEmployeesOrdersReport(string lunchMenuId)
        {
            var service = new CodeMashRepository<MenuEntity>(Client);
            var menuInfo = await service.AggregateAsync<PersonalOrdersAggregate>(Guid.Parse("6c11c9df-5738-4560-b8e3-c7d7f559285c"), new AggregateOptions
            {
                Tokens = new Dictionary<string, string>()
                {
                       { "id", lunchMenuId },
                }
            });

            return menuInfo[0];
        }
        public async Task<List<WishlistSummary>> GetWhishListSummary(DateTime dateFrom, DateTime dateTo)
        {
            var taxRepo = new CodeMashTermsService(Client);
            var taxonomyData = await taxRepo.FindAsync("wishlist-types", x => true, new TermsFindOptions());

            var from = ((DateTimeOffset)dateFrom).ToUnixTimeMilliseconds();
            var to = ((DateTimeOffset)dateTo).ToUnixTimeMilliseconds();
            var service = new CodeMashRepository<WishlistEntity>(Client);
            var whishlistSummary = await service.AggregateAsync<object>(Guid.Parse("9ffca22e-eb98-4e65-b843-283389ac8c53"), new AggregateOptions
            {
                Tokens = new Dictionary<string, string>()
                {
                    { "dateFrom", from.ToString() },
                    { "dateTo", to.ToString() },
                }
            });

            var list = new List<WishlistSummary>();
            foreach (var elem in whishlistSummary)
            {
                var resultJson = JObject.Parse(elem.ToString());
                var id = resultJson["_id"].ToString();
                var total = resultJson["total"].ToString();
                foreach (var tax in taxonomyData.Items)
                {
                    if(id == tax.Id)
                    {
                        list.Add(new WishlistSummary { Type = tax.Name + "-", Total= total + " EUR" });
                        break;
                    }
                }
            }
            
            return list;
        }
    }
}
