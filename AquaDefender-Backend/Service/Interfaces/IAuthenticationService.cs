using AquaDefender_Backend.Domain;

namespace AquaDefender_Backend.Service.Interfaces
{
    public interface IAuthenticationService
    {
        string CreateToken(AppUser user);
    }
}