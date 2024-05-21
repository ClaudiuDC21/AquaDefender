using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.DTOs;
using AquaDefender_Backend.Services.Interfaces;
using AquaDefender_Backend.Service.Interfaces;
using System.IO.Compression;

namespace AquaDefender_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(IReportService reportService, IWebHostEnvironment webHostEnvironment)
        {
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{reportId}")]
        public async Task<IActionResult> GetReportById(int reportId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reportId <= 0)
            {
                return BadRequest("Id-ul raportului trebuie să fie un număr întreg pozitiv.");
            }

            var report = await _reportService.GetReportByIdAsync(reportId);
            if (report == null)
            {
                return NotFound("Raportul nu a fost găsit.");
            }

            return Ok(report);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReports()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reports = await _reportService.GetAllReportsAsync();
            return Ok(reports);
        }

        [HttpGet("images")]
        public async Task<ActionResult<List<ReportImage>>> GetAllImagesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var images = await _reportService.GetAllImagesAsync();
            return Ok(images);
        }

        [HttpGet("{reportId}/images")]
        public async Task<ActionResult<List<ReportImage>>> GetImagesByReportIdAsync(int reportId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reportId <= 0)
            {
                return BadRequest("Id-ul raportului trebuie să fie un număr întreg pozitiv.");
            }

            var images = await _reportService.GetImagesByReportIdAsync(reportId);

            if (images == null || images.Count == 0)
            {
                return NotFound("Imaginile nu au fost găsite.");
            }

            var imageStreams = new List<MemoryStream>();

            foreach (var image in images)
            {
                var imagePath = Path.Combine(image.ImageUrl);

                if (System.IO.File.Exists(imagePath))
                {
                    var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
                    var imageStream = new MemoryStream(imageBytes);
                    imageStreams.Add(imageStream);
                }
            }

            if (imageStreams.Count == 0)
            {
                return NotFound("Imaginile nu au fost găsite.");
            }

            var archiveStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
            {
                for (int i = 0; i < imageStreams.Count; i++)
                {
                    var entry = zipArchive.CreateEntry($"image_{i + 1}.jpg");
                    using (var entryStream = entry.Open())
                    {
                        imageStreams[i].CopyTo(entryStream);
                    }
                }
            }

            archiveStream.Position = 0;

            return File(archiveStream, "application/zip", $"images_{reportId}.zip");
        }

        [HttpGet("{reportId}/hasimages")]
        public async Task<ActionResult<bool>> ReportHasImages(int reportId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reportId <= 0)
            {
                return BadRequest("Id-ul raportului trebuie să fie un număr întreg pozitiv.");
            }

            var hasImages = await _reportService.ReportHasImagesAsync(reportId);
            return Ok(hasImages);
        }

        [HttpGet("User/{userId}/Total")]
        public async Task<ActionResult<int>> GetTotalReportsByUserId(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId <= 0)
            {
                return BadRequest("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            var totalReports = await _reportService.GetReportCountByUserIdAsync(userId);
            return Ok(totalReports);
        }

        [HttpGet("User/{userId}/New")]
        public async Task<ActionResult<int>> GetNewReportsByUserId(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId <= 0)
            {
                return BadRequest("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            var newReportsCount = await _reportService.GetNewReportCountByUserIdAsync(userId);
            return Ok(newReportsCount);
        }

        [HttpGet("User/{userId}/InProgress")]
        public async Task<ActionResult<int>> GetInProgressReportsByUserId(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId <= 0)
            {
                return BadRequest("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            var inProgressReportsCount = await _reportService.GetInProgressReportCountByUserIdAsync(userId);
            return Ok(inProgressReportsCount);
        }

        [HttpGet("User/{userId}/Resolved")]
        public async Task<ActionResult<int>> GetResolvedReportsByUserId(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId <= 0)
            {
                return BadRequest("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            var resolvedReportsCount = await _reportService.GetResolvedReportCountByUserIdAsync(userId);
            return Ok(resolvedReportsCount);
        }

        [HttpGet("City/{cityId}/New")]
        public async Task<ActionResult<int>> GetNewReportCountByCityId(int cityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (cityId <= 0)
            {
                return BadRequest("Id-ul orașului trebuie să fie un număr întreg pozitiv.");
            }

            int count = await _reportService.GetNewReportCountByCityIdAsync(cityId);
            return Ok(count);
        }


        [HttpGet("stats/{cityId}")]
        public async Task<IActionResult> GetReportStats(int cityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (cityId <= 0)
            {
                return BadRequest("Id-ul orașului trebuie să fie un număr întreg pozitiv.");
            }

            var stats = await _reportService.GetReportStatisticsByCityIdAsync(cityId);
            return Ok(stats);
        }

        [HttpGet("user/status/severity")]
        public async Task<IActionResult> GetFilteredReportsbyUserId([FromQuery] int userId, [FromQuery] ReportStatus? status, [FromQuery] SeverityLevel? severity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId <= 0)
            {
                return BadRequest("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            var reports = await _reportService.GetFilteredReportsByUserId(userId, status, severity);
            return Ok(reports);
        }

        [HttpGet("random-new")]
        public async Task<IActionResult> GetRandomNewReports()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reports = await _reportService.GetRandomNewReports();
            return Ok(reports);
        }

        // Get randomly selected in-progress reports
        [HttpGet("random-in-progress")]
        public async Task<IActionResult> GetRandomInProgressReports()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reports = await _reportService.GetRandomInProgressReports();
            return Ok(reports);
        }

        // Get randomly selected completed reports
        [HttpGet("random-completed")]
        public async Task<IActionResult> GetRandomCompletedReports()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reports = await _reportService.GetRandomCompletedReports();
            return Ok(reports);
        }

        [HttpGet("filteredReportsByCityId")]
        public async Task<IActionResult> GetFilteredReportsbyCityId([FromQuery] int cityId, [FromQuery] ReportStatus? status, [FromQuery] SeverityLevel? severity, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string userName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (cityId <= 0)
            {
                return BadRequest("Id-ul orașului trebuie să fie un număr întreg pozitiv.");
            }

            var reports = await _reportService.GetFilteredReportsByCityId(cityId, status, severity, startDate, endDate, userName);
            return Ok(reports);
        }

        [HttpPut("{reportId}/status")]
        public async Task<IActionResult> UpdateReportStatus(int reportId, [FromQuery] UpdateStatusDto updateStatusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reportId <= 0)
            {
                return BadRequest("Id-ul raportului trebuie să fie un număr întreg pozitiv.");
            }

            if (updateStatusDto == null)
            {
                return BadRequest("Statusul nu poate fi gol.");
            }

            var updatedReport = await _reportService.UpdateReportStatusAsync(reportId, updateStatusDto.Status);
            if (updatedReport == null)
            {
                return NotFound($"Raportul cu ID-ul {reportId} nu a fost găsit sau nu a putut fi actualizat.");
            }

            return Ok(updatedReport);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReportWithImages([FromForm] ReportDto reportDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var report = new Report
                {
                    Title = reportDto.Title,
                    Description = reportDto.Description,
                    ReportDate = DateTime.Now, // Setez ora curentă indiferent de inputul DTO
                    UserId = reportDto.UserId,
                    CountyId = reportDto.CountyId,
                    CityId = reportDto.CityId,
                    LocationDetails = reportDto.LocationDetails,
                    Latitude = reportDto.Latitude,
                    Longitude = reportDto.Longitude,
                    IsAnonymous = reportDto.IsAnonymous,
                    Status = reportDto.Status,
                    Severity = reportDto.Severity
                };

                // Verificați dacă există imagini pentru încărcare
                List<string> savedImagePaths = null;
                if (reportDto.Images != null && reportDto.Images.Count > 0)
                {
                    savedImagePaths = await SaveImages(reportDto.Images, _webHostEnvironment.WebRootPath);
                }

                await _reportService.CreateReportAsync(report, savedImagePaths);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(reportDto);
        }

        [HttpPut("{reportId}")]
        public async Task<IActionResult> UpdateReport(int reportId, [FromForm] ReportDto reportDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reportId <= 0)
            {
                return BadRequest("Id-ul raportului trebuie să fie un număr întreg pozitiv.");
            }

            var existingReport = await _reportService.GetReportByIdAsync(reportId);
            if (existingReport == null)
            {
                return NotFound($"Raportul cu ID-ul {reportId} nu a fost găsit.");
            }

            try
            {
                // Update existing report with DTO properties
                existingReport.Title = reportDto.Title;
                existingReport.Description = reportDto.Description;
                existingReport.ReportDate = reportDto.ReportDate;
                existingReport.CountyId = reportDto.CountyId;
                existingReport.CityId = reportDto.CityId;
                existingReport.LocationDetails = reportDto.LocationDetails;
                existingReport.Latitude = reportDto.Latitude;
                existingReport.Longitude = reportDto.Longitude;
                existingReport.IsAnonymous = reportDto.IsAnonymous;
                existingReport.Status = reportDto.Status;
                existingReport.Severity = reportDto.Severity;

                List<string> savedImagePaths = null;
                if (reportDto.Images != null && reportDto.Images.Count > 0)
                {
                    savedImagePaths = await SaveImages(reportDto.Images, _webHostEnvironment.WebRootPath);
                }
                await _reportService.UpdateReportAsync(existingReport, savedImagePaths);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(reportDto);
        }

        [HttpDelete("{reportId}")]
        public async Task<IActionResult> DeleteReport(int reportId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reportId <= 0)
            {
                return BadRequest("Id-ul raportului trebuie să fie un număr întreg pozitiv.");
            }

            var report = await _reportService.GetReportByIdAsync(reportId);
            if (report == null)
            {
                return NotFound($"Raportul cu ID-ul {reportId} nu a fost găsit.");
            }

            await _reportService.DeleteReportAsync(reportId);
            return NoContent();
        }

        private async Task<List<string>> SaveImages(List<IFormFile> images, string webRootPath)
        {
            List<string> savedImagePaths = new List<string>();

            foreach (var image in images)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
                var filePath = Path.Combine(webRootPath, "reportImages", uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                savedImagePaths.Add(filePath);
            }

            return savedImagePaths;
        }

    }
}
