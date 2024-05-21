using AquaDefender_Backend.Data;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.DTOs;
using AquaDefender_Backend.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AquaDefender_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersController(IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUserById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound($"Utilizatorul cu ID-ul {id} nu a fost găsit.");
            }

            return Ok(user);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromForm] UserUpdateDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId <= 0)
            {
                return BadRequest("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            var existingUser = await _userService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound($"Utilizatorul cu ID-ul {userId} nu a fost găsit.");
            }

            try
            {
                if (!string.IsNullOrEmpty(userDto.UserName))
                {
                    existingUser.UserName = userDto.UserName;
                }

                if (userDto.BirthDate.HasValue)
                {
                    existingUser.BirthDate = userDto.BirthDate.Value;
                }

                // Assuming registration date should not be updated
                existingUser.RegistrationDate = existingUser.RegistrationDate;

                if (!string.IsNullOrEmpty(userDto.PhoneNumber))
                {
                    existingUser.PhoneNumber = userDto.PhoneNumber;
                }

                if (userDto.CountyId.HasValue && userDto.CityId.HasValue)
                {
                    existingUser.CountyId = userDto.CountyId.Value;
                    existingUser.CityId = userDto.CityId.Value;
                }
                else if (userDto.CountyId.HasValue || userDto.CityId.HasValue)
                {
                    return BadRequest("Atât CountyId cât și CityId trebuie furnizate împreună.");
                }

                // Check if a new profile picture is uploaded
                if (userDto.ProfilePicture != null)
                {
                    // Logic to save the profile picture, returning the path to the image
                    var profileImagePath = await SaveImage(userDto.ProfilePicture, _webHostEnvironment.WebRootPath);
                    // Assuming we have a property for the image path in the user entity
                    existingUser.ProfilePicture = profileImagePath;
                }

                await _userService.UpdateUserAsync(existingUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(existingUser);
        }

        [HttpPut("{userId}/update-password")]
        public async Task<IActionResult> UpdatePassword(int userId, [FromBody] PasswordChangeDto passwordChangeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId <= 0)
            {
                return BadRequest("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            if (string.IsNullOrEmpty(passwordChangeDto.OldPassword))
            {
                return BadRequest("Parola veche nu trebuie să fie goală.");
            }
            if (string.IsNullOrEmpty(passwordChangeDto.NewPassword))
            {
                return BadRequest("Parola nouă nu trebuie să fie goală.");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Utilizatorul cu ID-ul {userId} nu a fost găsit.");
            }

            // Verify the old password
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedOldPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordChangeDto.OldPassword));
            if (!computedOldPasswordHash.SequenceEqual(user.PasswordHash))
            {
                return BadRequest("Parola veche este incorectă.");
            }

            // Update to the new password
            using var newHmac = new HMACSHA512();
            user.PasswordHash = newHmac.ComputeHash(Encoding.UTF8.GetBytes(passwordChangeDto.NewPassword));
            user.PasswordSalt = newHmac.Key;

            try
            {
                await _userService.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Eșec la actualizarea parolei utilizatorului: {ex.Message}");
            }

            return Ok("Parola a fost actualizată cu succes.");
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId <= 0)
            {
                return BadRequest("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Utilizatorul cu ID-ul {userId} nu a fost găsit.");
            }

            try
            {
                await _userService.DeleteUserAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<string> SaveImage(IFormFile image, string webRootPath)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
            var filePath = Path.Combine(webRootPath, "profilePictureImages", uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return filePath;
        }


    }
}