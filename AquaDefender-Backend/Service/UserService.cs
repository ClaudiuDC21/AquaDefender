using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using AquaDefender_Backend.Service.Interfaces;

namespace AquaDefender_Backend.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<AppUser> GetUserByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"Utilizatorul cu id-ul {userId} nu există.");
            }

            return user;
        }

        public async Task<List<AppUser>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task UpdateUserAsync(AppUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Utilizatorul nu poate fi nul.");
            }

            var existingUser = await _userRepository.GetUserByIdAsync(user.Id);
            if (existingUser == null)
            {
                throw new ArgumentException($"Utilizatorul cu id-ul {user.Id} nu există.");
            }

            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.", nameof(userId));
            }

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"Utilizatorul cu id-ul {userId} nu există.");
            }

            await _userRepository.DeleteUserAsync(userId);
        }

    }
}
