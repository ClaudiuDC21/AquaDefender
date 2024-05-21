using System;
using Microsoft.AspNetCore.Http;

namespace AquaDefender_Backend.DTOs
{
    public class UserUpdateDto
    {
        public string UserName { get; set; }
        public DateTime? BirthDate { get; set; } // Nullable DateTime
        public string PhoneNumber { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public int? CountyId { get; set; } // Nullable int
        public int? CityId { get; set; } // Nullable int
    }
}
