using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.DTOs;

namespace AquaDefender_Backend.Service.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}