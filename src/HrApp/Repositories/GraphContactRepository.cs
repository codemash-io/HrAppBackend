using Microsoft.Graph;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HrApp
{
    public class GraphContactRepository : IGraphContactRepository
    {
        private readonly GraphRepository graphRepository = new GraphRepository();

        public async Task<List<Contact>> GetAllUserContacts(string userId, string expand = null, string select = null)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

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


            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(graphUrl);
                if (!response.IsSuccessStatusCode)
                    throw new BusinessException("Something Please check your input data and try again.");

                var resultString = await response.Content.ReadAsStringAsync();
                var resultJson = JObject.Parse(resultString);
                var contactsDetails = resultJson["value"].ToString();

                var contacts = JsonConvert.DeserializeObject<List<Contact>>(contactsDetails);
                return contacts;
            }
        }

        public async Task<Contact> GetUserContactById(string userId, string contactId, string expand = null, string select = null)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

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

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(graphUrl);
                if (!response.IsSuccessStatusCode)
                    throw new BusinessException("Something went wrong. Please check your input data and try again.");

                var resultString = await response.Content.ReadAsStringAsync();
                var contact = JsonConvert.DeserializeObject<Contact>(resultString);
                return contact;
            }
        }
        public async Task<Contact> CreateUserContact(string userId, Contact contact)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var jsonBody = JsonConvert.SerializeObject(contact);
            var graphUrl = graphRepository.BaseGraphUrl + "/users/"
                + userId + "/contacts";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PostAsync(graphUrl,
                    new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                var resultString = await response.Content.ReadAsStringAsync();
                var createdContact = JsonConvert.DeserializeObject<Contact>(resultString);

                if (response.IsSuccessStatusCode)
                    return createdContact;
                else
                    throw new BusinessException("Something went wrong. " +
                        "Contact was not created. Please check your input data and try again.");
            }
        }

        public async Task<Contact> UpdateUserContact(string userId, string contactId, Contact contact)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var jsonBody = JsonConvert.SerializeObject(contact);
            var graphUrl = graphRepository.BaseGraphUrl + "/users/"
                + userId + "/contacts/" + contactId;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PatchAsync(graphUrl,
                    new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new BusinessException("Something went wrong. " +
                        "Contact was not updated. Please check your input data and try again.");

                var resultString = await response.Content.ReadAsStringAsync();
                var updatedContact = JsonConvert.DeserializeObject<Contact>(resultString);
                return updatedContact;
            }
        }
        public async Task<bool> DeleteUserContact(string userId, string contactId)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var graphUrl = graphRepository.BaseGraphUrl + "/users/"
                + userId + "/contacts/" + contactId;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.DeleteAsync(graphUrl);

                if (!response.IsSuccessStatusCode)
                    throw new BusinessException("Something went wrong. " +
                        "Contact was not updated. Please check your input data and try again.");

                return true;
            }
        }
    }
}
