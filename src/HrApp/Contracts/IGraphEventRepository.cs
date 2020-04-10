using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IGraphEventRepository
    {
        Task<List<Event>> GetCalendarEventsByDate(string roomName, DateTime from, DateTime to);
        Task<string> CreateEvent(Event calendatEvent);
        Task<bool> EditEventById(string eventId, Event calendarEvent);
        Task<bool> DeleteEventById(string eventId, string roomName);
    }
}
