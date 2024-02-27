using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Data;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AquaDefender_Backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AquaDefenderDataContext _dbContext;

        public UserRepository(AquaDefenderDataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<AppUser> GetUserByIdAsync(int userId)
        {
            // Asynchronously retrieves a user by ID from the database using EF Core's FindAsync.
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<List<AppUser>> GetAllUsersAsync()
        {
            // Asynchronously retrieves all users from the database using EF Core's ToListAsync.
            return await _dbContext.Users.ToListAsync();
        }
    }
}