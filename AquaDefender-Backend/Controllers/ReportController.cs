using Microsoft.AspNetCore.Mvc;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.DTOs;
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
        private readonly ILogger<ReportController> _logger;

        public ReportController(IReportService reportService, IWebHostEnvironment webHostEnvironment, ILogger<ReportController> logger)
        {
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

            try
            {
                var report = await _reportService.GetReportByIdAsync(reportId);
                if (report == null)
                {
                    return NotFound("Raportul nu a fost găsit.");
                }

                return Ok(report);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, $"An error occurred while getting the report with ID {reportId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReports()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var reports = await _reportService.GetAllReportsAsync();
                return Ok(reports);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while getting all reports.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("images")]
        public async Task<ActionResult<List<ReportImage>>> GetAllImagesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var images = await _reportService.GetAllImagesAsync();
                return Ok(images);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while getting all images.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{reportId}/images")]
        public async Task<ActionResult> GetImagesByReportIdAsync(int reportId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reportId <= 0)
            {
                return BadRequest("Id-ul raportului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting images for report ID {reportId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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

            try
            {
                var hasImages = await _reportService.ReportHasImagesAsync(reportId);
                return Ok(hasImages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while checking if the report with ID {reportId} has images.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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

            try
            {
                var totalReports = await _reportService.GetReportCountByUserIdAsync(userId);
                return Ok(totalReports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the total report count for user with ID {userId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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

            try
            {
                var newReportsCount = await _reportService.GetNewReportCountByUserIdAsync(userId);
                return Ok(newReportsCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the new report count for user with ID {userId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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

            try
            {
                var inProgressReportsCount = await _reportService.GetInProgressReportCountByUserIdAsync(userId);
                return Ok(inProgressReportsCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the in-progress report count for user with ID {userId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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

            try
            {
                var resolvedReportsCount = await _reportService.GetResolvedReportCountByUserIdAsync(userId);
                return Ok(resolvedReportsCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the resolved report count for user with ID {userId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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

            try
            {
                int count = await _reportService.GetNewReportCountByCityIdAsync(cityId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the new report count for city with ID {cityId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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

            try
            {
                var stats = await _reportService.GetReportStatisticsByCityIdAsync(cityId);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting report statistics for city with ID {cityId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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

            try
            {
                var reports = await _reportService.GetFilteredReportsByUserId(userId, status, severity);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting filtered reports for user with ID {userId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("random-new")]
        public async Task<IActionResult> GetRandomNewReports()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var reports = await _reportService.GetRandomNewReports();
                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting random new reports.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("random-in-progress")]
        public async Task<IActionResult> GetRandomInProgressReports()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var reports = await _reportService.GetRandomInProgressReports();
                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting random in-progress reports.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("random-completed")]
        public async Task<IActionResult> GetRandomCompletedReports()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var reports = await _reportService.GetRandomCompletedReports();
                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting random completed reports.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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

            try
            {
                var reports = await _reportService.GetFilteredReportsByCityId(cityId, status, severity, startDate, endDate, userName);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting filtered reports for city with ID {cityId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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

            try
            {
                var updatedReport = await _reportService.UpdateReportStatusAsync(reportId, updateStatusDto.Status);
                if (updatedReport == null)
                {
                    return NotFound($"Raportul cu ID-ul {reportId} nu a fost găsit sau nu a putut fi actualizat.");
                }

                return Ok(updatedReport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the status of the report with ID {reportId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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
                    ReportDate = DateTime.Now, // Set current time regardless of DTO input
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

                // Check if there are images to upload
                List<string> savedImagePaths = null;
                if (reportDto.Images != null && reportDto.Images.Count > 0)
                {
                    savedImagePaths = await SaveImages(reportDto.Images, _webHostEnvironment.WebRootPath);
                }

                await _reportService.CreateReportAsync(report, savedImagePaths);
                return Ok(reportDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the report with images.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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
                return Ok(reportDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the report with ID {reportId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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

            try
            {
                await _reportService.DeleteReportAsync(reportId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the report with ID {reportId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        private async Task<List<string>> SaveImages(List<IFormFile> images, string webRootPath)
        {
            List<string> savedImagePaths = new List<string>();

            foreach (var image in images)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
                var filePath = Path.Combine(webRootPath, "reportImages", uniqueFileName);
                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    savedImagePaths.Add(filePath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while saving an image.");
                    throw; // Rethrow the exception to be caught in the calling method
                }
            }

            return savedImagePaths;
        }
    }
}
