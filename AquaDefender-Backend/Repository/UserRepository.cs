using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AquaDefender_Backend.Data;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AquaDefender_Backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AquaDefenderDataContext _dbContext;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AquaDefenderDataContext dbContext, ILogger<UserRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<AppUser> GetUserByIdAsync(int userId)
        {
            try
            {
                return await _dbContext.Users.FindAsync(userId);
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
                return await _dbContext.Users.ToListAsync();
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
                throw new ArgumentNullException(nameof(user));

            try
            {
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the user with ID {user.Id}.");
                throw;
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(userId);
                if (user != null)
                {
                    _dbContext.Users.Remove(user);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the user with ID {userId}.");
                throw;
            }
        }
    }
}
