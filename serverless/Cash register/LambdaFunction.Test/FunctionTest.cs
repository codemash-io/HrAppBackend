using System.Linq;
using CodeMash.Client;
using CodeMash.Repository;
using HrApp.Domain;
using HrApp.Entities;
using NUnit.Framework;
using System.IO;

namespace LambdaFunction
{
    [TestFixture]
    public class FunctionTests
    {
        #region variables
        public string TestResult =
            "{\"CompetencyLevel\":\"Senior\",\"Phones\":[{\"model\":\"Nokia 6.1\",\"phone_number\":\"864206969\",\"note\":\"labai geras telefonas\",\"_id\":\"5e3966a2e7676c00018b1840\"}],\"Computers\":[{\"Code\":\"K11\",\"Type\":\"Laptop\",\"Model\":\"Lenovo\",\"CPU\":null,\"RAM\":0,\"HDD\":[],\"Notes\":[],\"_id\":\"5e25ba6f1137ae00018e2a65\"}],\"Monitors\":[{\"code\":\"M11\",\"model\":\"DELL\",\"inches\":0,\"_id\":\"5e25c0b31137ae0001917564\"},{\"code\":\"M14\",\"model\":\"DELL\",\"inches\":0,\"_id\":\"5e25bf401137ae0001912913\"},{\"code\":\"M12\",\"model\":\"DELL\",\"inches\":0,\"_id\":\"5e26a0bfa326bc0001055e1b\"}],\"Trips\":[{\"Country\":\"Netherlands\",\"From\":\"2020-02-04\",\"To\":\"2020-02-13\"}],\"Trainings\":[{\"Amount\":{\"value\":5,\"currency\":\"EUR\"},\"Date\":\"2020-02-18\",\"Description\":\"Book C# in Depth\"}],\"CashPurchases\":[{\"Amount\":{\"value\":50,\"currency\":\"EUR\"},\"Date\":\"2020-02-10\",\"Description\":\"Ausines\"}],\"TrainingsCash\":{\"value\":800,\"currency\":\"EUR\"},\"TrainingsCashLeft\":{\"value\":795,\"currency\":\"EUR\"},\"BudgetFund\":{\"value\":240,\"currency\":\"EUR\"},\"BudgetFundLeft\":{\"value\":190,\"currency\":\"EUR\"}}";
        public static CodeMashClient Client = new CodeMashClient("96WLxsvp7FNolruRNIMYycgVT7rI4_Et", HrApp.Settings.ProjectId);
        #endregion

        [Test]
        [TestCase("5d35b974e609490001b0fcb3", ExpectedResult = "Junior")]
        [TestCase("5d35b97ce609490001b0fcc0", ExpectedResult = "Junior / Medium")]
        [TestCase("5d35b984e609490001b0fcc9", ExpectedResult = "Medium")]
        [TestCase("5d35b98be609490001b0fcda", ExpectedResult = "Medium / Senior")]
        [TestCase("5d35b995e609490001b0fce7", ExpectedResult = "Senior")]
        [TestCase("5e2fdd885cd4fc0001347ecf", ExpectedResult = "Team Lead")]
        public string CompetencyLevelTaxonomyTests(string taxonomyId)
        {
            var taxonomy = new CodeMashTermsService(Client);

            var competencies = taxonomy.Find<CompetencyLevelMeta>("competency-level", entity => true);
            var competencyLevel = competencies.List.First(x => x.Id == taxonomyId).Name;

            return competencyLevel;
        }

        [Test]
        [TestCase("5d357618e609490001b0f391", ExpectedResult = "Netherlands")]
        [TestCase("5d357629e609490001b0f3ab", ExpectedResult = "Germany")]
        [TestCase("5d357623e609490001b0f39e", ExpectedResult = "Lithuania")]
        public string MapCountryTest(string countryTaxonomyId)
        {
            var trip = new Trip { Country = countryTaxonomyId };

            trip.MapCountry();

            return trip.Country;
        }

        [Test]
        //this tests checks whether fields in my object
        //get null values or not, when reading from db
        public void ReadFromDb()
        {
            var collection = new CodeMashRepository<UserDbEntity>(Client);

            var items = collection.Find().Items;

            var monitorSum = items.Sum(x => x.Monitors.Count);
            var computerSum = items.Sum(x => x.Computers.Count);
            var phonesSum = items.Sum(x => x.Phones.Count);
            var cashSum = items.Sum(x => x.Cash.Count);
            var trainSum = items.Sum(x => x.Trainings.Count);
            var travelSum = items.Sum(x => x.business_trips.Count);
            var levels = items.Sum(x => x.level.Length);

            Assert.AreNotEqual(0, monitorSum);
            Assert.AreNotEqual(0, computerSum);
            Assert.AreNotEqual(0, phonesSum);
            Assert.AreNotEqual(0, cashSum);
            Assert.AreNotEqual(0, trainSum);
            Assert.AreNotEqual(0, travelSum);
            Assert.AreNotEqual(0, levels);
        }

        [Test]
        //this test sees whether assetIDs are even mapped to anything
        public void MapAssetsTest()
        {
            var repo = new CodeMashRepository<UserDbEntity>(Client);
            var assetRepo = new CodeMashRepository<PhoneEntity>(Client);

            var pcEmployee = repo.Find().Items.OrderByDescending(x => x.Phones.Count).First();

            var monitors = pcEmployee.Computers.Select(x => assetRepo.FindOneById(x)).ToList();

            Assert.AreNotEqual(monitors, null);
        }

        [Test]
        [TestCase("353e5ee9-f7f8-47b1-a93e-69b4ddd72c2f")]
        public void TestLambdaFunction(string applicationUserId)
        {
            var function = new Function();
            var result = function.Handler(applicationUserId);
            var resultJson = result.Body;

            Assert.AreEqual(TestResult, resultJson);
        }
    }
}