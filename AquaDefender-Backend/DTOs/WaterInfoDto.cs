namespace AquaDefender_Backend.DTOs
{
    public class WaterInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public DateTime DateReported { get; set; }
        public string AdditionalNotes { get; set; }
    }
}