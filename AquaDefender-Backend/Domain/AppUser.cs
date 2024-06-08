using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AquaDefender_Backend.Domain
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public int RoleId { get; set; }
        public UserRole Role { get; set; }
        public DateTime BirthDate { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public string ProfilePicture { get; set; }
        [Required]
        public int CountyId { get; set; }
        public County County { get; set; }
        [Required]
        public int CityId { get; set; }
        public City City { get; set; }
        [JsonIgnore]
        public List<Report> Reports { get; set; }
    }
}