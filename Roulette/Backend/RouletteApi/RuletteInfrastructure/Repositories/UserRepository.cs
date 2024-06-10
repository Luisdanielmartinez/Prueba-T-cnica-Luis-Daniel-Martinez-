using Microsoft.EntityFrameworkCore;
using RuletteCore.Entities;
using RuletteCore.Interfaces;
using RuletteInfrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuletteInfrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RouletteContext _context;

        public UserRepository(RouletteContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
