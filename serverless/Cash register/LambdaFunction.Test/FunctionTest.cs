using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using CodeMash.Client;
using CodeMash.Repository;
using HrApp;
using HrApp.Domain;
using HrApp.Entities;
using Isidos.CodeMash.ServiceContracts;
using LambdaFunction.Inputs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace LambdaFunction
{
    [TestFixture]
    public class FunctionTests
    {
        public static CodeMashClient client = new CodeMashClient("96WLxsvp7FNolruRNIMYycgVT7rI4_Et", HrApp.Settings.ProjectId);
        #region json
        public static string inputString =
            "{\r  \"photo\": [\r\n" +
            "\"1a60f905-88f9-437c-a281-9a70bf1c690e\"\r\n  ],\r\n" +
            "  \"division\": \"5d88ae84a792110001fef326\",\r\n" +
            "  \"sex\": \"Male\",\r\n" +
            "  \"first_name\": \"Domantas\",\r\n" +
            "  \"last_name\": \"Jovaišas\",\r\n" +
            "  \"notes\": [],\r\n" +
            "  \"level\": \"5d35b995e609490001b0fce7\",\r\n" +
            "  \"phones\": [],\r\n" +
            "  \"contract_type\": \"1 FTE\",\r\n" +
            "  \"education\": [\r\n" +
            "    {\r\n" +
            "      \"institution_name\": \"KTU\",\r\n" +
            "      \"info\": \"INgo\",\r\n" +
            "      \"from\": 2008,\r\n" +
            "      \"to\": 2018\r\n" +
            "    }\r\n" +
            "  ],\r\n" +
            "  \"position\": \"5d35b515e609490001b0f8b1\",\r\n" +
            "  \"city\": \"5d88b390a792110001fefa5b\",\r\n" +
            "  \"manager\": \"5d8ca4aa07f6fe000114ba01\",\r\n" +
            "  \"phone\": \"+37065012789\",\r\n" +
            "  \"birth_date\": 401083200000,\r\n" +
            "  \"address\": \"J. Barkausko 23\",\r\n" +
            "  \"customers\": [],\r\n" +
            "  \"activation\": [\r\n" +
            "    {\r\n" +
            "      \"is_active\": true,\r\n" +
            "      \"activation_is_done\": false,\r\n" +
            "      \"position\": \"5d35b515e609490001b0f8b1\",\r\n" +
            "      \"competency_level\": \"5d35b96de609490001b0fcaa\"\r\n" +
            "    }\r\n" +
            "  ],\r\n" +
            "  \"files\": [\r\n" +
            "    {\r\n" +
            "      \"comment\": \"CV\",\r\n" +
            "      \"user\": \"4db186cd-91b9-4538-97b5-40b0f7a139ab\",\r\n" +
            "      \"files\": [\r\n" +
            "        \"811b262c-d0b4-4f4b-bdbb-ea494afcedae\"\r\n" +
            "      ]\r\n" +
            "    }\r\n" +
            "  ],\r\n" +
            "  \"computers\": [\r\n" +
            "    \"5e25ba6f1137ae00018e2a65\"\r\n" +
            "  ],\r\n" +
            "  \"number_of_holidays_left\": 24,\r\n  \"monitors\": [\r\n    \"5e25c0b31137ae0001917564\",\r\n    \"5e25bf401137ae0001912913\",\r\n    \"5e26a0bfa326bc0001055e1b\"\r\n  ],\r\n  \"application_user\": \"353e5ee9-f7f8-47b1-a93e-69b4ddd72c2f\",\r\n  \"personal_identification_number\": \"38209170865\",\r\n  \"personal_email\": \"domantasjovaisas@gmail.com\",\r\n  \"business_email\": \"domantas.jovaisas@presentconnection.eu\",\r\n  \"hired_date\": 1349319600000,\r\n  \"children\": [],\r\n  \"health_information_booklet_valid_to\": 1615946400000,\r\n  \"cv\": [\r\n    \"0fca55cc-d7c9-4a62-8fbd-a38ffef85002\"\r\n  ],\r\n  \"business_trips\": [\r\n    {\r\n      \"country\": \"5d357618e609490001b0f391\",\r\n      \"from\": 1580781600000,\r\n      \"to\": 1581559200000\r\n    }\r\n  ],\r\n  \"emergency_contact\": [\r\n    {\r\n      \"name\": \"Kristina\",\r\n      \"position\": \"Wife\",\r\n      \"phone\": \"+37067348050\"\r\n    }\r\n  ],\r\n  \"cash\": [\r\n    {\r\n      \"amount\": {\r\n        \"value\": 50,\r\n        \"currency\": \"EUR\"\r\n      },\r\n      \"date\": 1581300000000,\r\n      \"description\": \"Ausines\"\r\n    }\r\n  ],\r\n  \"trainings\": [\r\n    {\r\n      \"amount\": {\r\n        \"value\": 5,\r\n        \"currency\": \"EUR\"\r\n      },\r\n      \"date\": 1581991200000,\r\n      \"description\": \"Book C# in Depth\"\r\n    }\r\n  ],\r\n  \"_id\": \"5d8ca4aa07f6fe000114ba01\"\r\n}";
        #endregion
        [Test(ExpectedResult = "Netherlands")]
        public string CashTests()
        {
            var taxonomy = new CodeMashTermsService(client);

            var competencies = taxonomy.Find<CompetencyLevelMeta>("CompetencyLevel", entity => true);
            //var junior = competencies.List.First(x => x.Meta.Name)

            return competencies.List.First().Meta.NetPrice.Value.ToString();
        }

        [Test]
        public void MapCountryTest()
        {
            var trip = new Trip
            {
                Country = "5d357618e609490001b0f391" //taxonomy ID for Netherlands
            };

            trip.MapCountry(client);

            Assert.AreEqual("Netherlands", trip.Country);
        }
    }
}