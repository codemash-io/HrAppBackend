using Microsoft.Graph;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public class GraphEventsRepository : IGraphEventRepository
    {
        private readonly GraphRepository graphRepository = new GraphRepository();
        /// <summary>
        /// Lists all calendar events by selected room
        /// </summary>
        /// <param name="roomName">Room name</param>
        /// <param name="from">Date from in UTC time</param>
        /// <param name="to">Date to in UTC time</param>
        /// <returns></returns>
        public async Task<List<Event>> GetCalendarEventsByDate(string roomName, DateTime from, DateTime to)
        {
            var roomId = await graphRepository.GetMeetingRoomId(roomName);

            var dateFrom = from.ToString("yyyy-MM-ddTHH:mm:ss");
            var dateTo = to.ToString("yyyy-MM-ddTHH:mm:ss");

            var graphUrl = graphRepository.BaseGraphUrl + "/users/" + roomId + "/calendarView?startDateTime="
                + dateFrom + "&endDateTime=" + dateTo;

            var resultString = await graphRepository.Get(graphUrl);
            var resultJson = JObject.Parse(resultString);
            var eventDetails = resultJson["value"].ToString();
            var events = JsonConvert.DeserializeObject<List<Event>>(eventDetails);
            return events;
        }

        public async Task<string> CreateEvent(Event calendarEvent)
        {
            var roomId = await graphRepository.GetMeetingRoomId(calendarEvent.Location.DisplayName);
            var graphUrl = graphRepository.BaseGraphUrl + "/users/" + roomId + "/events";

            var resultString = await graphRepository.Post(graphUrl, calendarEvent);

            dynamic resultJson = JObject.Parse(resultString);
            string cretedEventId = Convert.ToString(resultJson.id);
            return cretedEventId;           
        }
        public async Task<bool> DeleteEventById(string eventId, string roomName)
        {
            var roomId = await graphRepository.GetMeetingRoomId(roomName);
            var graphUrl = graphRepository.BaseGraphUrl + "/users/" + roomId + "/events/" + eventId;
            return await graphRepository.Delete(graphUrl);
        }

        public async Task<bool> EditEventById(string eventId, Event calendarEvent)
        {
            var roomId = await graphRepository.GetMeetingRoomId(calendarEvent.Location.DisplayName);
            var graphUrl = graphRepository.BaseGraphUrl + "/users/" + roomId + "/events/" + eventId;
            
            var resultString = await graphRepository.Patch(graphUrl, calendarEvent);
            var updatedEvent = JsonConvert.DeserializeObject<Event>(resultString);
            if (updatedEvent != null)
                return true;
            else
                return false;

        }
    }
}
