using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AquaDefender_Backend.Data;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.DTOs;
using AquaDefender_Backend.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AquaDefender_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AquaDefenderDataContext _context;
        private readonly IAuthenticationService _authenticationService;


        public AuthenticationController(AquaDefenderDataContext context,
                                        IAuthenticationService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await EmailExists(registerDto.Email))
            {
                return BadRequest("Email-ul este deja inregistrat");
            }
            using var hmac = new HMACSHA512();

            int roleId = 2; 
            var domain = registerDto.Email.Split('@').Last().ToLower();

            var county = _context.County.Include(c => c.Cities).FirstOrDefault(c => c.Id == int.Parse(registerDto.County));
            var city = county?.Cities.FirstOrDefault(c => c.Id == int.Parse(registerDto.City));

            if (county == null || city == null)
            {
                return BadRequest("Județul sau orașul specificat nu există.");
            }

            if (county != null && county.WaterDeptEmail != null && domain == county.WaterDeptEmail.Split('@').Last().ToLower())
            {
                if (registerDto.Email != county.WaterDeptEmail)
                {
                    return BadRequest("Email-ul specificat nu corespunde departamentului de apă al județului ales.");
                }
                roleId = 4; 
            }
            else if (city != null && city.CityHallEmail != null && domain == city.CityHallEmail.Split('@').Last().ToLower())
            {
                if (registerDto.Email != city.CityHallEmail)
                {
                    return BadRequest("Email-ul specificat nu corespunde primăriei orașului ales.");
                }
                roleId = 3;
            }

            var user = new AppUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                CountyId = int.Parse(registerDto.County),
                CityId = int.Parse(registerDto.City),
                RoleId = roleId,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                RoleId = user.RoleId,
                Token = _authenticationService.CreateToken(user),
                CityId = user.CityId
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null) return Unauthorized("Email-ul nu este corect");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Parola nu este corecta");
            }

            return new UserDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                RoleId = user.RoleId,
                Token = _authenticationService.CreateToken(user),
                CityId = user.CityId
            };
        }


        private async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email.ToLower());
        }

    }
}