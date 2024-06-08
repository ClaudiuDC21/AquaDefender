using AquaDefender_Backend.Domain;

namespace AquaDefender_Backend.Service.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> GetUserByIdAsync(int userId);
        Task<List<AppUser>> GetAllUsersAsync();
        Task UpdateUserAsync(AppUser user);
        Task DeleteUserAsync(int userId);
        Task<bool> HasProfilePictureAsync(int userId);
    }
}