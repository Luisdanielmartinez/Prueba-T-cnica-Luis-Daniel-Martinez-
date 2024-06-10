using RuletteCore.Entities;
using RuletteCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuletteApplication.Handlers
{
    public class BetHandler
    {
        private readonly IUserRepository _userRepository;
        private static readonly string[] Colors = new[] { "Red", "Black" };

        public BetHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<decimal> HandleBetAsync(Bet bet)
        {
            try
            {
                var random = new Random();
                var number = random.Next(0, 37);
                var color = Colors[random.Next(0, Colors.Length)];

                var user = await _userRepository.GetUserByNameAsync(bet.UserName);
                if (user == null)
                {
                    user = new User { Name = bet.UserName, Amount = 0 };
                    await _userRepository.AddUserAsync(user);
                }

                decimal prize = 0;
                if (bet.Number.HasValue && bet.Number.Value == number && bet.Color.ToLower() == color.ToLower())
                {
                    prize = bet.Amount * 3;
                }
                else if (bet.Color.ToLower() == color.ToLower())
                {
                    prize = bet.Amount / 2;
                }
                else if ((bet.IsEven.HasValue && bet.IsEven.Value == (number % 2 == 0)) && bet.Color.ToLower() == color.ToLower())
                {
                    prize = bet.Amount;
                }

                user.Amount += prize - bet.Amount;
                await _userRepository.UpdateUserAsync(user);

                return prize;
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging framework)
                Console.Error.WriteLine($"Error handling bet: {ex.Message}");
                throw new ApplicationException("An error occurred while processing the bet.", ex);
            }
        }

        public async Task SaveUserAsync(User user)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByNameAsync(user.Name);
                if (existingUser == null)
                {
                    await _userRepository.AddUserAsync(user);
                }
                else
                {
                    existingUser.Amount += user.Amount;
                    await _userRepository.UpdateUserAsync(existingUser);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging framework)
                Console.Error.WriteLine($"Error saving user: {ex.Message}");
                throw new ApplicationException("An error occurred while saving the user.", ex);
            }
        }

        public async Task<User> GetUserByName(string name)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByNameAsync(name);
                return existingUser;
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging framework)
                Console.Error.WriteLine($"Error getting user by name: {ex.Message}");
                throw new ApplicationException("An error occurred while retrieving the user.", ex);
            }
        }
    }
}
