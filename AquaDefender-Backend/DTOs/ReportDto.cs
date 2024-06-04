using AquaDefender_Backend.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AquaDefender_Backend.DTOs
{
    public class ReportDto
    {
        [Required(ErrorMessage = "Titlul este obligatoriu.")]
        [MaxLength(120, ErrorMessage = "Titlul trebuie să aibă cel mult 120 de caractere.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Descrierea este obligatorie.")]
        [MaxLength(400, ErrorMessage = "Descrierea trebuie să aibă cel mult 400 de caractere.")]
        public string Description { get; set; }

        public DateTime ReportDate { get; set; } = DateTime.Now;

        public int UserId { get; set; }

        [Required(ErrorMessage = "Județul este obligatoriu.")]
        public int CountyId { get; set; }

        [Required(ErrorMessage = "Localitatea este obligatoriu.")]
        public int CityId { get; set; }

        public string LocationDetails { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsAnonymous { get; set; }
        public ReportStatus Status { get; set; }

        [Required(ErrorMessage = "Nivelul de severitate este obligatoriu.")]
        public SeverityLevel Severity { get; set; }

        public List<IFormFile> Images { get; set; }
    }
}
