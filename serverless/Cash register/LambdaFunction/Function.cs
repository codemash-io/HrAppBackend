using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using CodeMash.Repository;
using HrApp.Domain;
using HrApp.Entities;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaFunction
{
    public class Function
    {
        public APIGatewayProxyResponse Handler(string applicationUserId)
        {
            var usersRepo = new CodeMashRepository<UserDbEntity>(HrApp.Settings.Client);
            var monitorsRepo = new CodeMashRepository<MonitorEntity>(HrApp.Settings.Client);
            var computersRepo = new CodeMashRepository<ComputerEntity>(HrApp.Settings.Client);
            var phonesRepo = new CodeMashRepository<PhoneEntity>(HrApp.Settings.Client);
            var taxonomy = new CodeMashTermsService(HrApp.Settings.Client);

            var userRecord = usersRepo.FindOne(x => x.ApplicationUser == applicationUserId);
            userRecord.business_trips.ForEach(x => x.MapCountry());

            var competencyLevel = taxonomy.Find<CompetencyLevelMeta>("competency-level", entity => true)
                .List.First(x => x.Id == userRecord.level);
            var monitors = userRecord.Monitors.Select(x => monitorsRepo.FindOneById(x)).ToList();
            var computers = userRecord.Computers.Select(x => computersRepo.FindOneById(x)).ToList();
            var phones = userRecord.Phones.Select(x => phonesRepo.FindOneById(x)).ToList();

            var trainingsCash = new Price
            {
                Value = competencyLevel.Meta.Training.Value - userRecord.Trainings.Sum(x => x.Amount.Value),
                Currency = competencyLevel.Meta.Training.Currency
            };

            var budgetFundCash = new Price
            {
                Value = competencyLevel.Meta.BudgetFund.Value - userRecord.Cash.Sum(x => x.Amount.Value),
                Currency = competencyLevel.Meta.BudgetFund.Currency
            };

            var employeeDto = new EmployeeDTO
            {
                CompetencyLevel = competencyLevel.Name,
                Computers = computers,
                Monitors = monitors,
                Phones = phones,
                Trainings = userRecord.Trainings.Select(x => x.GetCashDto()).ToList(),
                Trips = userRecord.business_trips.Select(x => x.GeTripDto()).ToList(),
                CashPurchases = userRecord.Cash.Select(x => x.GetCashDto()).ToList(),
                TrainingsCash = competencyLevel.Meta.Training,
                TrainingsCashLeft = trainingsCash,
                BudgetFund = competencyLevel.Meta.BudgetFund,
                BudgetFundLeft = budgetFundCash
            };

            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(employeeDto),
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
            };
        }
    }
}