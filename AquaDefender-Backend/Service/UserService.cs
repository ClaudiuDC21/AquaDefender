using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using AquaDefender_Backend.Service.Interfaces;

namespace AquaDefender_Backend.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<AppUser> GetUserByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException($"Utilizatorul cu id-ul {userId} nu există.");
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the user with ID {userId}.");
                throw;
            }
        }

        public async Task<List<AppUser>> GetAllUsersAsync()
        {
            try
            {
                return await _userRepository.GetAllUsersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all users.");
                throw;
            }
        }

        public async Task UpdateUserAsync(AppUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Utilizatorul nu poate fi nul.");
            }

            try
            {
                var existingUser = await _userRepository.GetUserByIdAsync(user.Id);
                if (existingUser == null)
                {
                    throw new ArgumentException($"Utilizatorul cu id-ul {user.Id} nu există.");
                }

                await _userRepository.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the user with ID {user.Id}.");
                throw;
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.", nameof(userId));
            }

            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException($"Utilizatorul cu id-ul {userId} nu există.");
                }

                await _userRepository.DeleteUserAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the user with ID {userId}.");
                throw;
            }
        }

        public async Task<bool> HasProfilePictureAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.", nameof(userId));
            }

            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException($"Utilizatorul cu id-ul {userId} nu există.");
                }

                return !string.IsNullOrEmpty(user.ProfilePicture);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while checking if the user with ID {userId} has a profile picture.");
                throw;
            }
        }
    }
}
