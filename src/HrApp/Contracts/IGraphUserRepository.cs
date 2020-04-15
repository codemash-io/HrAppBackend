using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphUser = Microsoft.Graph.User;

namespace HrApp
{
    public interface IGraphUserRepository
    {
        Task<List<GraphUser>> GetAllGraphUsers();
        Task<GraphUser> GetGraphUserById(string userId);
        Task<bool> EditGraphUser(string userId, GraphUser userDetails);
        Task<List<Event>> GetUserReminderView(string userId, DateTime from, DateTime to);
        Task<GraphUser> CreateGraphUser(GraphUser newUser);
        Task<bool> EditGraphUserAvatar(string userId, byte[] imageStream);
        Task<byte[]> GetUserProfilePhoto(string userId, string size);
    }
}
