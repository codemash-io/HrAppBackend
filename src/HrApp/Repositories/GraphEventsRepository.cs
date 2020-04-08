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
    public class GraphEventsRepository : IGraphEventRepository
    {
        private readonly GraphRepository graphRepository = new GraphRepository();
        public async Task<List<Event>> GetCalendarEventsByDate(string roomName, DateTime from, DateTime to)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var roomId = await graphRepository.GetMeetingRoomId(roomName);

            var dateFrom = from.ToUniversalTime().ToString();
            var dateTo = to.ToUniversalTime().ToString();

            var graphUrl = graphRepository.BaseGraphUrl + "/users/" + roomId + "/calendarView?startDateTime="
                + dateFrom + "&endDateTime=" + dateTo;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(graphUrl);
                var resultString = await response.Content.ReadAsStringAsync();

                dynamic resultJson = JObject.Parse(resultString);
                var eventDetails = resultJson.value;
                var events = graphRepository.MapCalendarEvent(eventDetails);

                return events;
            }
        }

        public async Task<string> CreateEvent(Event calendarEvent)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var roomId = await graphRepository.GetMeetingRoomId(calendarEvent.Location.DisplayName);
            //var calendarDetails = await GetSelectedRoomCalendarDetails(calendarEvent.Location.DisplayName);

            //var jsonBody = FormEventJosnBody(calendarDetails, calendarEvent);
            var jsonBody = JsonConvert.SerializeObject(calendarEvent);

            var graphUrl = graphRepository.BaseGraphUrl + "/users/" + roomId + "/events";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PostAsync(graphUrl,
                    new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                var resultString = await response.Content.ReadAsStringAsync();
                dynamic resultJson = JObject.Parse(resultString);
                string cretedEventId = Convert.ToString(resultJson.id);
                return cretedEventId;
            }
        }
        public async Task<bool> DeleteEventById(string eventId, string roomName)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var roomId = await graphRepository.GetMeetingRoomId(roomName);

            var graphUrl = graphRepository.BaseGraphUrl + "/users/" + roomId + "/events/" + eventId;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.DeleteAsync(graphUrl);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }

        public async Task<bool> EditEventById(string eventId, Event calendarEvent)
        {
            var token = Environment.GetEnvironmentVariable("token");
            if (string.IsNullOrEmpty(token))
                token = await graphRepository.GetAccessToken();

            var roomId = await graphRepository.GetMeetingRoomId(calendarEvent.Location.DisplayName);

            var jsonBody = JsonConvert.SerializeObject(calendarEvent);

            var graphUrl = graphRepository.BaseGraphUrl + "/users/" + roomId + "/events/" + eventId;

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
