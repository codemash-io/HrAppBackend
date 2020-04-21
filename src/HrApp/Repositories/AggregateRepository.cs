using CodeMash.Client;
using CodeMash.Repository;
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
    }
}
