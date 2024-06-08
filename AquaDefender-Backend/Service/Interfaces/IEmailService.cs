using AquaDefender_Backend.DTOs;

namespace AquaDefender_Backend.Service.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}