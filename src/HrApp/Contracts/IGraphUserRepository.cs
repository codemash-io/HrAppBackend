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
    }
}
