using InWestyGator.WebDemo.Core.Contracts;
using InWestyGator.WebDemo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InWestyGator.WebDemo.DataAccess.Repositories
{
    // this is a mock, no logging
    public class MockUserRepository : IUserRepository
    {
        private List<User> _users = new List<User>
        {
            new User
            {
                Id = 0, Username = "peter", Password = "peter123"
            },
            new User
            {
                Id = 1, Username = "john", Password = "john123"
            },
            new User
            {
                Id = 2, Username = "james", Password = "james123"
            }
        };
        public Task<User> AuthenticateAsync(string username, string password)
        {
            var user = _users.SingleOrDefault(u => u.Username == username);
            if (user == null) { return Task.FromResult<User>(null); }
            if (user.Password == password)
            {
                return Task.FromResult<User>(user);
            }
            return Task.FromResult<User>(null);
        }

        public Task<User> GetUserAsync(int id)
        {
            var user = _users[id];
            if (user == null) throw new ArgumentException();
            // just for sanity
            user.Password = "";
            return Task.FromResult(user);
        }

        public async Task<List<string>> GetUserNamesAsync()
        {
            var users = new List<string>();
            foreach (var user in _users)
            {
                users.Add(user.Username);
            }
            return await Task.FromResult(users);
        }
    }
}
