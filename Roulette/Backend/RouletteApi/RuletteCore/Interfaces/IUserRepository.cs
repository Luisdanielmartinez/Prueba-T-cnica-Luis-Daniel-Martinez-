using RuletteCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuletteCore.Interfaces
{
        public interface IUserRepository
        {
            Task<User> GetUserByNameAsync(string name);
            Task AddUserAsync(User user);
            Task UpdateUserAsync(User user);
        }
}
