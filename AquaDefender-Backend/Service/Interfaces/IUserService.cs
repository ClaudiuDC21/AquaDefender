using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Domain;

namespace AquaDefender_Backend.Service.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> GetUserByIdAsync(int userId);

        Task<List<AppUser>> GetAllUsersAsync();
    }
}