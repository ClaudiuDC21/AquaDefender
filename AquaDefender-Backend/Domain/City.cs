using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AquaDefender_Backend.Domain
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountyId { get; set; }
        [JsonIgnore]
        public County County { get; set; }
        public string CityHallEmail { get; set; }
    }
}