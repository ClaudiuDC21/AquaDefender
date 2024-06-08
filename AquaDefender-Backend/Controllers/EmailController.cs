using AquaDefender_Backend.DTOs;
using AquaDefender_Backend.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AquaDefender_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendEmail(EmailDto request)
        {
            _emailService.SendEmail(request);

            return Ok();
        }

    }
}