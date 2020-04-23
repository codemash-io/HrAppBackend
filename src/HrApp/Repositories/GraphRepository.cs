using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using CodeMash.Client;
using CodeMash.Project.Services;
using Isidos.CodeMash.ServiceContracts;
using System.Text;
using Newtonsoft.Json;

namespace HrApp
{
    public class GraphRepository : IGraphRepository
    {
        public readonly string BaseGraphUrl  = "https://graph.microsoft.com/v1.0";
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);
        private Dictionary<string, string> LoadAppSettings()
        {
            var appConfig = new Dictionary<string, string>
            {
                { "appId", Settings.ClientId },
                { "appSecret", Settings.AppSecret },
                { "tenantId", Settings.TenantId }
            };

            // Check for required settings
            if (string.IsNullOrEmpty(appConfig["appId"]) ||
                string.IsNullOrEmpty(appConfig["appSecret"]) ||
                string.IsNullOrEmpty(appConfig["tenantId"]))
                throw new BusinessException("App settings not set");

            return appConfig;
        }   

        public async Task<string> GetAccessToken()
        {
            var appConfig = LoadAppSettings();
            var tokenEndPoint = "https://login.microsoftonline.com/";
            tokenEndPoint += appConfig["tenantId"] + "/oauth2/v2.0/token";

            var values = new Dictionary<string, string>
            {
                { "client_id", appConfig["appId"] },
                { "scope", "https://graph.microsoft.com/.default" },
                { "client_secret", appConfig["appSecret"] },
                { "grant_type", "client_credentials" },
            };

            using (var httpClient = new HttpClient())
            {
                using (var content = new FormUrlEncodedContent(values))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    var response = await httpClient.PostAsync(tokenEndPoint, content);
                    var resultString = await response.Content.ReadAsStringAsync();
                    var resultJson = JObject.Parse(resultString);

                    string token = string.Empty;
                    if (string.IsNullOrEmpty(resultJson["access_token"].ToString()))
                        throw new BusinessException("Access token is empty");

                    token = resultJson["access_token"].ToString();

                    Environment.SetEnvironmentVariable("token", token);
                    return token.ToString();
                }
            }
        }

        public async Task<RoomDetails> GetSelectedRoomCalendarDetails(string meetingRoom)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if(string.IsNullOrEmpty(token))
                token = await GetAccessToken();

            var roomId = await GetMeetingRoomId(meetingRoom);
            var graphUrl = BaseGraphUrl + "/users/" + roomId;
            
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(graphUrl + "/calendar");
                var resultString = await response.Content.ReadAsStringAsync();
                dynamic resultJson = JObject.Parse(resultString);

                var room = new RoomDetails
                {
                    Email = resultJson.owner.address,
                    Name = resultJson.owner.name,
                    Id = resultJson.id
                };

                return room;
            }         
        }
        public async Task<string> GetMeetingRoomId(string room)
        {
            var projectService = new CodeMashProjectService(Client);

            var settings = await projectService.GetProjectAsync(new GetProjectRequest());
            var tokens = settings.Result.Tokens;

            foreach (var token in tokens)
            {
                if (token.Key.ToLower() == room.ToLower())
                    return token.Value;
            }
            throw new BusinessException("No such a room exists");
        }

        public async Task<string> Get(string graphUrl)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await GetAccessToken();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(graphUrl);
                if (!response.IsSuccessStatusCode)
                    throw new BusinessException("Something went wrong. Please check your input data and try again.");

                var resultString = await response.Content.ReadAsStringAsync();
                return resultString;
            }
        }
        public async Task<string> Post(string graphUrl, object body)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await GetAccessToken();

            var jsonBody = JsonConvert.SerializeObject(body);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PostAsync(graphUrl,
                    new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new BusinessException("Something went wrong. Please check your input data and try again.");

                var resultString = await response.Content.ReadAsStringAsync();
                return resultString;
            }
        }

        public async Task<string> Put(string graphUrl, object body)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await GetAccessToken();

            var jsonBody = JsonConvert.SerializeObject(body);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PutAsync(graphUrl,
                    new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new BusinessException("Something went wrong. Please check your input data and try again.");

                var resultString = await response.Content.ReadAsStringAsync();
                return resultString;
            }
        }
        public async Task<string> Patch(string graphUrl, object body)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await GetAccessToken();

            var jsonBody = JsonConvert.SerializeObject(body);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PatchAsync(graphUrl,
                    new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new BusinessException("Something went wrong. Please check your input data and try again.");

                var resultString = await response.Content.ReadAsStringAsync();
                return resultString;
            }
        }
        public async Task<bool> Delete(string graphUrl)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await GetAccessToken();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.DeleteAsync(graphUrl);

                if (!response.IsSuccessStatusCode)
                    throw new BusinessException("Something went wrong. Please check your input data and try again.");

                return true;
            }
        }

    }
}
