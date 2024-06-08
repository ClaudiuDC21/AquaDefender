using System.ComponentModel.DataAnnotations;

namespace AquaDefender_Backend.Domain
{
    public class ReportImage
    {
        [Key]
        public int IdImage { get; set; }
        public string ImageUrl { get; set; }
        public int ReportId { get; set; }
        public Report Report { get; set; }
    }

}