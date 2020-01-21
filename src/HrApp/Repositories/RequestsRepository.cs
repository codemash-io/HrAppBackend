using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Repository;

namespace HrApp.Repositories
{
    public class RequestsRepository: IRequestsRepository
    {
        public void AddRequest(RequestEntity request)
        {
            var repo = new CodeMashRepository<RequestEntity>(Settings.Client);

            var testRequest = new RequestEntity
            {
                Description = request.Description,
                Price = request.Price,
                //Links = new List<string>{ "http://www.google.com", "http://www.amazon.com" },
                RequestType = request.RequestType
            };

            repo.InsertOne(testRequest);
        }
    }
}
