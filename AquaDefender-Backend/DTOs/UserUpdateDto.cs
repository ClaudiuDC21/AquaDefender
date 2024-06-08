namespace AquaDefender_Backend.DTOs
{
    public class UserUpdateDto
    {
        public string UserName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public int? CountyId { get; set; }
        public int? CityId { get; set; }
    }
}
