using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IGraphRepository
    {
        Task<string> GetAccessToken();
        Task<List<Event>> GetCalendarEventsByDate(string roomName, DateTime from, DateTime to);
        Task<List<Event>> GetAllCalendarEvents(string roomName);
        Task<string> CreateEvent(Event calendatEvent);
        Task<Office365User> GetOffice365UserById(string userId);
        Task<bool> DeleteEventById(string eventId, string roomName);
        Task<RoomDetails> GetSelectedRoomCalendarDetails(string meetingRoom);
        Task<bool> EditEventById(string eventId, Event calendarEvent);
    }
}
