using Microsoft.Graph;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<GraphUser> CreateGraphUser(GraphUser newUser)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var jsonBody = JsonConvert.SerializeObject(newUser);
            var graphUrl = graphRepository.BaseGraphUrl + "/users";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PostAsync(graphUrl,
                    new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                var resultString = await response.Content.ReadAsStringAsync();

                var createdUser = JsonConvert.DeserializeObject<GraphUser>(resultString);
                if (response.IsSuccessStatusCode)
                    return createdUser;
                else
                    throw new BusinessException("Something went wrong. User was not created");
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

        public async Task<bool> EditGraphUserAvatar(string userId, byte[] imageStream)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var byteArrayContent = new ByteArrayContent(imageStream.ToArray());
            byteArrayContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/jpeg");

            var graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId + "/photo/$value";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/jpeg"));

                var response = await httpClient.PutAsync(graphUrl, byteArrayContent);

                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// Lists all selected user reminders
        /// </summary>
        /// <param name="userId">Selected user ID</param>
        /// <param name="from">Date from in UTC time</param>
        /// <param name="to">Date to in UTC time</param>
        /// <returns></returns>
        public async Task<List<Event>> GetUserReminderView(string userId, DateTime from, DateTime to)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var dateFrom = from.ToString("yyyy-MM-ddTHH:mm:ss");
            var dateTo = to.ToString("yyyy-MM-ddTHH:mm:ss");

            var graphUrl = graphRepository.BaseGraphUrl + "/users/" + userId +
                "/reminderView(startDateTime='" + dateFrom + 
                "',endDateTime='" + dateTo + "')";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(graphUrl);
                if (!response.IsSuccessStatusCode)
                    throw new BusinessException("Something went wrong. Check your input data");

                var resultString = await response.Content.ReadAsStringAsync();
                var resultJson = JObject.Parse(resultString);

                var eventDetails = resultJson["value"].ToString();
                var reminders = JsonConvert.DeserializeObject<List<Event>>(eventDetails);

                return reminders;        
            }
        }
    }
}
