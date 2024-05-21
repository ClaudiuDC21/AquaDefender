using AquaDefender_Backend.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AquaDefender_Backend.DTOs
{
    public class ReportDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime ReportDate { get; set; } = DateTime.Now;

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CountyId { get; set; }

        [Required]
        public int CityId { get; set; }

        public string LocationDetails { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
        public bool IsAnonymous { get; set; }

        [Required]
        public ReportStatus Status { get; set; }

        [Required]
        public SeverityLevel Severity { get; set; }

        public List<IFormFile> Images { get; set; }
    }
}
