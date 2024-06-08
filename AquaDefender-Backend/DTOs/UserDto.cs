namespace AquaDefender_Backend.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
        public int CityId { get; set; }
    }
}