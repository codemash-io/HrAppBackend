using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GraphUser = Microsoft.Graph.User;

namespace HrApp
{
    public class GraphUserRepository : IGraphUserRepository
    {
        private readonly GraphRepository graphRepository = new GraphRepository();

        public async Task<GraphUser> GetGraphUserById(string userId)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(graphUrl);

                var resultString = await response.Content.ReadAsStringAsync();

                var user = JsonConvert.DeserializeObject<GraphUser>(resultString);
                return user;
            }
        }

        public async Task<List<GraphUser>> GetAllGraphUsers()
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var graphUrl = graphRepository.BaseGraphUrl + "/users/";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(graphUrl);

                var resultString = await response.Content.ReadAsStringAsync();
                var resultJson = JObject.Parse(resultString);

                var eventDetails = resultJson["value"].ToString();

                var users = JsonConvert.DeserializeObject<List<GraphUser>>(eventDetails);
                return users;
            }
        }

        public async Task<bool> EditGraphUser(string userId, GraphUser userDetails)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var jsonBody = JsonConvert.SerializeObject(userDetails);

            var graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PatchAsync(graphUrl,
                    new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }
    }
}
