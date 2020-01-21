using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Client;
using CodeMash.Repository;
using HrApp.Domain;

namespace HrApp.Services
{
    public class RequestManager
    {
        public List<RequestType> GetRequestTypes()
        {
            var client = new CodeMashClient(Settings.ApiKey, Settings.ProjectId);
            var repo = new CodeMashRepository<RequestType>(client);

            return repo.Find().Items;
        }

        public void InsertRequest(/*EmployeeEntity employee, string description*/)
        {
            var repo = new CodeMashRepository<RequestEntity>(Settings.Client);
            var typeRepo = new CodeMashRepository<RequestType>(Settings.Client);

            var testRequest = new RequestEntity
                {
                    Description = "Lenovo Y510P",
                    Price = 250,
                    //Links = new List<string>{ "http://www.google.com", "http://www.amazon.com" },
                    RequestType = "5e18530f2fdb6d0001adeac4"
            };

            repo.InsertOne(testRequest);
        }
    }
}
