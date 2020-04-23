using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IAggregateRepository
    {
        Task<LunchOrderAggregate> LunchMenuReport(string lunchMenuId);
        Task<PersonalOrdersAggregate> LunchMenuEmployeesOrdersReport(string lunchMenuId);
        Task<List<object>> GetWhishListSummary(DateTime dateFrom, DateTime dateTo);
    }
}
