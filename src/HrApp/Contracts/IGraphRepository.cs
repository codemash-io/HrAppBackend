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
        Task<string> GetMeetingRoomId(string room);
        Task<string> Get(string graphUrl);
        Task<string> Post(string graphUrl, object body);
        Task<string> Put(string graphUrl, object body);
        Task<string> Patch(string graphUrl, object body);
        Task<bool> Delete(string graphUrl);
    }
}
