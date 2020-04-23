using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IGraphContactRepository
    {
        Task<List<Contact>> GetAllUserContacts(string userId, string expand = null, string select = null);
        Task<Contact> GetUserContactById(string userId, string contactId, string expand, string select);
        Task<Contact> CreateUserContact(string userId, Contact contact);
        Task<Contact> UpdateUserContact(string userId, string contactId, Contact contact);
        Task<bool> DeleteUserContact(string userId, string contactId);
    }
}
