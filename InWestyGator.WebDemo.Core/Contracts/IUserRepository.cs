using InWestyGator.WebDemo.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InWestyGator.WebDemo.Core.Contracts
{
    public interface IUserRepository
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> GetUserAsync(int id);
        Task<List<string>> GetUserNamesAsync();
    }
}
