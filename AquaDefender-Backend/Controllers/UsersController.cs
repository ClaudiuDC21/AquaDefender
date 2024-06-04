using AquaDefender_Backend.Data;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.DTOs;
using AquaDefender_Backend.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.IO.Compression;

namespace AquaDefender_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, IWebHostEnvironment webHostEnvironment, ILogger<UsersController> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all users.");
                return StatusCode(500, "Internal server error");
            }
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

            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"Utilizatorul cu ID-ul {id} nu a fost găsit.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the user with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
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

            try
            {
                var existingUser = await _userService.GetUserByIdAsync(userId);
                if (existingUser == null)
                {
                    return NotFound($"Utilizatorul cu ID-ul {userId} nu a fost găsit.");
                }

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
                    var profileImagePath = await SaveImage(userDto.ProfilePicture, _webHostEnvironment.WebRootPath);
                    // Assuming we have a property for the image path in the user entity
                    existingUser.ProfilePicture = profileImagePath;
                }

                await _userService.UpdateUserAsync(existingUser);
                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the user with ID {userId}.");
                return StatusCode(500, "Internal server error");
            }
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

            try
            {
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

                await _userService.UpdateUserAsync(user);
                return Ok("Parola a fost actualizată cu succes.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the password for user with ID {userId}.");
                return StatusCode(500, "Internal server error");
            }
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

            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound($"Utilizatorul cu ID-ul {userId} nu a fost găsit.");
                }

                await _userService.DeleteUserAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the user with ID {userId}.");
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<string> SaveImage(IFormFile image, string webRootPath)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
            var filePath = Path.Combine(webRootPath, "profilePictureImages", uniqueFileName);

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                return filePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the profile picture.");
                throw;
            }
        }

        [HttpGet("{userId}/profileImage")]
        public async Task<IActionResult> GetUserProfileImage(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId <= 0)
            {
                return BadRequest("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("Imaginea de profil este nula.");
                }

                if(string.IsNullOrEmpty(user.ProfilePicture))
                {
                    return NotFound("Imaginea de profil este goala.");
                }

                var imageStreams = new List<MemoryStream>();
                // Assuming user.ProfilePicture contains a path like "imagesRoot\\profilePictureImages\\..."
                var imagePath = Path.Combine(user.ProfilePicture);

                // Log the constructed file path
                _logger.LogInformation($"Profile image path: {imagePath}");

                if (!System.IO.File.Exists(imagePath))
                {
                    // Log the file existence check
                    _logger.LogWarning($"Profile image not found at path: {imagePath}");
                    return NotFound("Imaginea de profil nu a fost găsită bine.");
                }

                
                var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
                var imageStream = new MemoryStream(imageBytes);
                imageStreams.Add(imageStream);

                var archiveStream = new MemoryStream();
                using (var zipArchive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < imageStreams.Count; i++)
                    {
                        var entry = zipArchive.CreateEntry($"profile_image_{i + 1}.jpg");
                        using (var entryStream = entry.Open())
                        {
                            imageStreams[i].CopyTo(entryStream);
                        }
                    }
                }

                archiveStream.Position = 0;

                return File(archiveStream, "application/zip", $"profile_image_{userId}.zip");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting profile image for user ID {userId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}
