using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphUser = Microsoft.Graph.User;

namespace HrApp
{
    public interface IGraphRepository
    {
        Task<string> GetAccessToken();
        Task<RoomDetails> GetSelectedRoomCalendarDetails(string meetingRoom);
        List<Event> MapCalendarEvent(dynamic eventDetails);
        Task<string> GetMeetingRoomId(string room);




    }
}
