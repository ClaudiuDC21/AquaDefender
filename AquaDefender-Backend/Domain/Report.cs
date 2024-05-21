using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AquaDefender_Backend.Domain
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReportDate { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public int CountyId { get; set; }
        public County County { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public string LocationDetails { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsAnonymous { get; set; }
        public List<ReportImage> Images { get; set; }
        public ReportStatus Status { get; set; }
        public SeverityLevel Severity { get; set; } // Nivelul de severitate al problemei
    }

    public enum ReportStatus
    {
        New,
        InProgress,
        Resolved
    }

    public enum SeverityLevel
    {
        Minor,
        Moderate,
        Serious,
        Severe,
        Critical
    }

}