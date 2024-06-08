using System.ComponentModel.DataAnnotations;

namespace AquaDefender_Backend.Domain
{
  public class WaterInfo
  {
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int CountyId { get; set; }
    public County County { get; set; }
    public int CityId { get; set; }
    public City City { get; set; }
    public DateTime DateReported { get; set; }
    public List<WaterValues> WaterValues { get; set; }
    public string AdditionalNotes { get; set; }

  }
}