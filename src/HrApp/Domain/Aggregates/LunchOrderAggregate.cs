using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public class LunchOrderAggregate
    {
        [Field("totalPrice")]
        public long TotalPrice { get; set; }

        [Field("totalCount")]
        public int TotalCount { get; set; }

        [Field("date")]
        public float Date { get; set; }

        [Field("meals")]
        public List<MealsByType> Meals { get; set; }
    }
}
