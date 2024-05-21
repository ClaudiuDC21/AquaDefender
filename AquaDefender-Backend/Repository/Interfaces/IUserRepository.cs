using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Domain;

namespace AquaDefender_Backend.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByIdAsync(int userId);
        Task<List<AppUser>> GetAllUsersAsync();
        Task UpdateUserAsync(AppUser user);
        Task DeleteUserAsync(int userId);
    }
}