namespace AquaDefender_Backend.DTOs
{
    public class WaterValuesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MaximumAllowedValue { get; set; }
        public string UserProvidedValue { get; set; }
        public string MeasurementUnit { get; set; }
        public int IdWaterInfo { get; set; }
    }
}