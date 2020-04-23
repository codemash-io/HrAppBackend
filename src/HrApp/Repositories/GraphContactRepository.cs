using Microsoft.Graph;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public class GraphContactRepository : IGraphContactRepository
    {
        private readonly GraphRepository graphRepository = new GraphRepository();

        public async Task<List<Contact>> GetAllUserContacts(string userId, string expand = null, string select = null)
        {
            string graphUrl;

            if (string.IsNullOrEmpty(expand) && string.IsNullOrEmpty(select))
                graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                    "/contacts/";
            else if (string.IsNullOrEmpty(expand) && !string.IsNullOrEmpty(select))
                graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                    "/contacts/" + "?$select=" + select;
            else if (!string.IsNullOrEmpty(expand) && string.IsNullOrEmpty(select))
                graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                    "/contacts/" + "?$expand=" + expand;
            else
                graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                    "/contacts/" + "?$expand=" + expand + "&$select=" + select;

            var resultString = await graphRepository.Get(graphUrl);
            var resultJson = JObject.Parse(resultString);
            var contactsDetails = resultJson["value"].ToString();

            var contacts = JsonConvert.DeserializeObject<List<Contact>>(contactsDetails);
            return contacts;
        }

        public async Task<Contact> GetUserContactById(string userId, string contactId, string expand = null, string select = null)
        {
            string graphUrl;

            if (string.IsNullOrEmpty(expand) && string.IsNullOrEmpty(select))
                graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                    "/contacts/" + contactId;
            else if (string.IsNullOrEmpty(expand) && !string.IsNullOrEmpty(select))
                graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                    "/contacts/" + contactId + "?$select=" + select;
            else if (!string.IsNullOrEmpty(expand) && string.IsNullOrEmpty(select))
                graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                    "/contacts/" + contactId + "?$expand=" + expand;
            else
                graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                    "/contacts/" + contactId + "?$expand=" + expand + "&$select=" + select;

            var resultString = await graphRepository.Get(graphUrl);
            var contact = JsonConvert.DeserializeObject<Contact>(resultString);
            return contact;
        }
        public async Task<Contact> CreateUserContact(string userId, Contact contact)
        {
            var graphUrl = graphRepository.BaseGraphUrl + "/users/"
                + userId + "/contacts";

            var resultString = await graphRepository.Post(graphUrl, contact);
            var createdContact = JsonConvert.DeserializeObject<Contact>(resultString);
            return createdContact;
        }

        public async Task<Contact> UpdateUserContact(string userId, string contactId, Contact contact)
        {
            var graphUrl = graphRepository.BaseGraphUrl + "/users/"
                + userId + "/contacts/" + contactId;

            var resultString = await graphRepository.Patch(graphUrl, contact);
            var updatedContact = JsonConvert.DeserializeObject<Contact>(resultString);
            return updatedContact;

        }
        public async Task<bool> DeleteUserContact(string userId, string contactId)
        {
            var graphUrl = graphRepository.BaseGraphUrl + "/users/"
                + userId + "/contacts/" + contactId;
            return await graphRepository.Delete(graphUrl);
        }
    }
}
