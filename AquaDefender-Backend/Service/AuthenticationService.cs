using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AquaDefender_Backend.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(IConfiguration config, ILogger<AuthenticationService> logger)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string CreateToken(AppUser user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
                };

                var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(7),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the token.");
                throw new ApplicationException("An error occurred while creating the token.", ex);
            }
        }
    }
}
