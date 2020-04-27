using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using LambdaFunction.Inputs;
using LambdaFunction.Services;
using HrApp;
using System.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaFunction
{
    public class Function
    {
        private readonly IExampleService _exampleService;
        
        // (Required if adding other constructors. Otherwise, optional.) A default constructor
        // called by Lambda. If you are adding your custom constructors,
        // default constructor with no parameters must be added
        public Function() : this (new ExampleService()) {}

        // (Optional) An example of injecting a service. As a default constructor is called by Lambda
        // this constructor has to be called from default constructor
        public Function(IExampleService exampleService)
        {
            _exampleService = exampleService;
        }
        
        /// <summary>
        /// (Required) Entry method of your Lambda function.
        /// </summary>
        /// <param name="lambdaEvent">Type returned from CodeMash</param>
        /// <param name="context">Context data of a function (function config)</param>
        /// <returns></returns>
        public async Task Handler(CustomEventRequest<BasicInput> lambdaEvent, ILambdaContext context)
        {
            List<string> emails;
            if (lambdaEvent.Input.Data != null)
            {
                ProcessDTO items = JsonConvert.DeserializeObject<ProcessDTO>(lambdaEvent.Input.Data);
                if (items.APiKey != null)
                    HrApp.Settings.ApiKey = items.APiKey;
                emails = items.Emails;
            }
            else
            {
                emails = Environment.GetEnvironmentVariable("emailsToSend").Split(',').ToList();
                if (Environment.GetEnvironmentVariable("apiKey") != null)
                    HrApp.Settings.ApiKey = Environment.GetEnvironmentVariable("apiKey");
            }

            if (HrApp.Settings.ApiKey == null)
                throw new BusinessException("ApiKey not set");
            if (emails == null)
                throw new BusinessException("EmailsToSend cannot be null or empty!");

            var aggregateRepo = new AggregateRepository();
            var notificationSender = new NotificationSender();

             //+20h because cm deserializes date in utc so i add 20 to make potential end of the day 
             //and to make sure that aggregate return correct day results
            var today = DateTime.Now.Date.AddHours(20);

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

        }
    }
}
