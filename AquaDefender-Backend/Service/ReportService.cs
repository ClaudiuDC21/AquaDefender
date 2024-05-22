using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.DTOs;
using AquaDefender_Backend.Repository.Interfaces;
using AquaDefender_Backend.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace AquaDefender_Backend.Service
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<ReportService> _logger;

        public ReportService(IReportRepository reportRepository, IUserRepository userRepository,
            ILocationRepository locationRepository, ILogger<ReportService> logger)
        {
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Report> GetReportByIdAsync(int reportId)
        {
            if (reportId <= 0)
                throw new ArgumentException("Id-ul raportului trebuie să fie un număr întreg pozitiv.");

            try
            {
                var report = await _reportRepository.GetReportByIdAsync(reportId);
                if (report == null)
                {
                    throw new ArgumentException($"Raportul cu id-ul {reportId} nu există.");
                }

                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the report with ID {reportId}.");
                throw;
            }
        }

        public async Task<List<Report>> GetAllReportsAsync()
        {
            try
            {
                return await _reportRepository.GetAllReportsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all reports.");
                throw;
            }
        }

        public async Task<List<Report>> GetReportsByStatusAsync(ReportStatus status)
        {
            if (!Enum.IsDefined(typeof(ReportStatus), status))
            {
                throw new ArgumentException("Statusul raportului este invalid.");
            }

            try
            {
                return await _reportRepository.GetReportsByStatusAsync(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting reports by status {status}.");
                throw;
            }
        }

        public async Task<int> GetReportCountByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                return await _reportRepository.GetReportCountByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the report count for user with ID {userId}.");
                throw;
            }
        }

        public async Task<int> GetNewReportCountByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                return await _reportRepository.GetNewReportCountByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the new report count for user with ID {userId}.");
                throw;
            }
        }

        public async Task<int> GetInProgressReportCountByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                return await _reportRepository.GetInProgressReportCountByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the in-progress report count for user with ID {userId}.");
                throw;
            }
        }

        public async Task<int> GetResolvedReportCountByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                return await _reportRepository.GetResolvedReportCountByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the resolved report count for user with ID {userId}.");
                throw;
            }
        }

        public async Task<List<Report>> GetRandomNewReports()
        {
            try
            {
                return await _reportRepository.GetRandomReportsByStatus(ReportStatus.New, 3);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting random new reports.");
                throw;
            }
        }

        public async Task<List<Report>> GetRandomInProgressReports()
        {
            try
            {
                return await _reportRepository.GetRandomReportsByStatus(ReportStatus.InProgress, 3);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting random in-progress reports.");
                throw;
            }
        }

        public async Task<List<Report>> GetRandomCompletedReports()
        {
            try
            {
                return await _reportRepository.GetRandomReportsByStatus(ReportStatus.Resolved, 3);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting random completed reports.");
                throw;
            }
        }

        public async Task<int> GetNewReportCountByCityIdAsync(int cityId)
        {
            if (cityId <= 0)
            {
                throw new ArgumentException("Id-ul orașului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                return await _reportRepository.GetNewReportCountByCityIdAsync(cityId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the new report count for city with ID {cityId}.");
                throw;
            }
        }

        public async Task<ReportStatisticsDto> GetReportStatisticsByCityIdAsync(int cityId)
        {
            if (cityId <= 0)
            {
                throw new ArgumentException("Id-ul orașului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                var stats = new ReportStatisticsDto
                {
                    TotalReports = await _reportRepository.GetReportCountByCityIdAsync(cityId),
                    NewReports = await _reportRepository.GetReportCountByCityIdAndStatusAsync(cityId, ReportStatus.New),
                    CasesInProgress = await _reportRepository.GetReportCountByCityIdAndStatusAsync(cityId, ReportStatus.InProgress),
                    ResolvedReports = await _reportRepository.GetReportCountByCityIdAndStatusAsync(cityId, ReportStatus.Resolved),
                    VeryLowSeverity = await _reportRepository.GetReportCountByCityIdAndSeverityAsync(cityId, SeverityLevel.Minor),
                    LowSeverity = await _reportRepository.GetReportCountByCityIdAndSeverityAsync(cityId, SeverityLevel.Moderate),
                    MediumSeverity = await _reportRepository.GetReportCountByCityIdAndSeverityAsync(cityId, SeverityLevel.Serious),
                    HighSeverity = await _reportRepository.GetReportCountByCityIdAndSeverityAsync(cityId, SeverityLevel.Severe),
                    VeryHighSeverity = await _reportRepository.GetReportCountByCityIdAndSeverityAsync(cityId, SeverityLevel.Critical)
                };

                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting report statistics for city with ID {cityId}.");
                throw;
            }
        }

        public async Task<IEnumerable<Report>> GetFilteredReportsByUserId(int userId, ReportStatus? status, SeverityLevel? severity)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Id-ul utilizatorului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                return await _reportRepository.GetReportsByUserAndFilters(userId, status, severity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting filtered reports for user with ID {userId}.");
                throw;
            }
        }

        public async Task<IEnumerable<Report>> GetFilteredReportsByCityId(int cityId, ReportStatus? status, SeverityLevel? severity, DateTime? startDate, DateTime? endDate, string userName)
        {
            if (cityId <= 0)
            {
                throw new ArgumentException("Id-ul orașului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                return await _reportRepository.GetReportsByCityAndFilters(cityId, status, severity, startDate, endDate, userName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting filtered reports for city with ID {cityId}.");
                throw;
            }
        }

        public async Task<Report> UpdateReportStatusAsync(int reportId, ReportStatus newStatus)
        {
            if (reportId <= 0)
            {
                throw new ArgumentException("Id-ul raportului trebuie să fie un număr întreg pozitiv.");
            }

            if (!Enum.IsDefined(typeof(ReportStatus), newStatus))
            {
                throw new ArgumentException("Statusul raportului este invalid.");
            }

            try
            {
                return await _reportRepository.UpdateReportStatusAsync(reportId, newStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the status of the report with ID {reportId}.");
                throw;
            }
        }

        public async Task CreateReportAsync(Report report, List<string> images)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report), "Obiectul raportului nu poate fi nul.");
            }

            try
            {
                var existingReportById = await _reportRepository.GetReportByIdAsync(report.Id);
                if (existingReportById != null)
                {
                    throw new InvalidOperationException("Un raport cu același Id deja există.");
                }

                var user = await _userRepository.GetUserByIdAsync(report.UserId);
                if (user == null)
                {
                    throw new InvalidOperationException("Utilizatorul specificat nu există.");
                }

                var county = await _locationRepository.GetCountyByIdAsync(report.CountyId);
                if (county == null)
                {
                    throw new InvalidOperationException("Județul specificat nu există.");
                }

                var city = await _locationRepository.GetCityByIdAsync(report.CityId);
                if (city == null)
                {
                    throw new InvalidOperationException("Orașul specificat nu există.");
                }

                await _reportRepository.CreateReportAsync(report);

                if (images != null)
                {
                    foreach (var image in images)
                    {
                        var reportImage = new ReportImage
                        {
                            ImageUrl = image,
                            ReportId = report.Id
                        };

                        await _reportRepository.CreateReportImageAsync(reportImage);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the report.");
                throw;
            }
        }

        public async Task UpdateReportAsync(Report report, List<string> savedImagePaths)
        {
            if (report == null || report.Id <= 0)
            {
                throw new ArgumentException("Obiectul raportului sau Id-ul raportului este invalid.");
            }

            try
            {
                var existingReport = await _reportRepository.GetReportByIdAsync(report.Id);
                if (existingReport == null)
                {
                    throw new InvalidOperationException($"Raportul cu Id-ul {report.Id} nu există.");
                }

                if (savedImagePaths != null)
                {
                    await _reportRepository.DeleteReportImageAsync(report.Id);
                    foreach (var image in savedImagePaths)
                    {
                        var reportImage = new ReportImage
                        {
                            ImageUrl = image,
                            ReportId = report.Id
                        };

                        await _reportRepository.CreateReportImageAsync(reportImage);
                    }
                }

                await _reportRepository.UpdateReportAsync(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the report with ID {report.Id}.");
                throw;
            }
        }

        public async Task DeleteReportAsync(int reportId)
        {
            if (reportId <= 0)
            {
                throw new ArgumentException("Id-ul raportului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                await _reportRepository.DeleteReportAsync(reportId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the report with ID {reportId}.");
                throw;
            }
        }

        public async Task<List<ReportImage>> GetAllImagesAsync()
        {
            try
            {
                return await _reportRepository.GetAllImagesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all images.");
                throw;
            }
        }

        public async Task<List<ReportImage>> GetImagesByReportIdAsync(int reportId)
        {
            if (reportId <= 0)
            {
                throw new ArgumentException("Id-ul raportului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                return await _reportRepository.GetImagesByReportIdAsync(reportId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting images for report with ID {reportId}.");
                throw;
            }
        }

        public async Task<bool> ReportHasImagesAsync(int reportId)
        {
            if (reportId <= 0)
            {
                throw new ArgumentException("Id-ul raportului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                return await _reportRepository.ReportHasImagesAsync(reportId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while checking if the report with ID {reportId} has images.");
                throw;
            }
        }
    }
}
