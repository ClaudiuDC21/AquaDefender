using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Data;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AquaDefender_Backend.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly AquaDefenderDataContext _dbContext;
        private readonly ILogger<ReportRepository> _logger;

        public ReportRepository(AquaDefenderDataContext dbContext, ILogger<ReportRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Report> GetReportByIdAsync(int reportId)
        {
            try
            {
                return await _dbContext.Reports.FindAsync(reportId);
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
                return await _dbContext.Reports.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all reports.");
                throw;
            }
        }

        public async Task<List<Report>> GetReportsByStatusAsync(ReportStatus status)
        {
            try
            {
                return await _dbContext.Reports
                    .Where(report => report.Status == status)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting reports by status {status}.");
                throw;
            }
        }

        public async Task<int> GetReportCountByUserIdAsync(int userId)
        {
            try
            {
                return await _dbContext.Reports
                                     .CountAsync(r => r.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting report count for user ID {userId}.");
                throw;
            }
        }

        public async Task<int> GetNewReportCountByUserIdAsync(int userId)
        {
            try
            {
                return await _dbContext.Reports
                                     .CountAsync(r => r.UserId == userId && r.Status == ReportStatus.New);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting new report count for user ID {userId}.");
                throw;
            }
        }

        public async Task<int> GetInProgressReportCountByUserIdAsync(int userId)
        {
            try
            {
                return await _dbContext.Reports
                                     .CountAsync(r => r.UserId == userId && r.Status == ReportStatus.InProgress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting in-progress report count for user ID {userId}.");
                throw;
            }
        }

        public async Task<int> GetResolvedReportCountByUserIdAsync(int userId)
        {
            try
            {
                return await _dbContext.Reports
                                     .CountAsync(r => r.UserId == userId && r.Status == ReportStatus.Resolved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting resolved report count for user ID {userId}.");
                throw;
            }
        }

        public async Task<int> GetReportCountByCityIdAsync(int cityId)
        {
            try
            {
                return await _dbContext.Reports.CountAsync(r => r.CityId == cityId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting report count for city ID {cityId}.");
                throw;
            }
        }

        public async Task<int> GetReportCountByCityIdAndStatusAsync(int cityId, ReportStatus status)
        {
            try
            {
                return await _dbContext.Reports.CountAsync(r => r.CityId == cityId && r.Status == status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting report count for city ID {cityId} and status {status}.");
                throw;
            }
        }

        public async Task<int> GetNewReportCountByCityIdAsync(int cityId)
        {
            try
            {
                return await _dbContext.Reports
                    .CountAsync(r => r.CityId == cityId && r.Status == ReportStatus.New);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting new report count for city ID {cityId}.");
                throw;
            }
        }

        public async Task<int> GetReportCountByCityIdAndSeverityAsync(int cityId, SeverityLevel severity)
        {
            try
            {
                return await _dbContext.Reports.CountAsync(r => r.CityId == cityId && r.Severity == severity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting report count for city ID {cityId} and severity {severity}.");
                throw;
            }
        }

        public async Task<List<Report>> GetRandomReportsByStatus(ReportStatus status, int count)
        {
            try
            {
                return await _dbContext.Reports
                                     .Where(r => r.Status == status)
                                     .OrderBy(r => Guid.NewGuid())
                                     .Take(count)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting random reports by status {status}.");
                throw;
            }
        }

        public async Task<IEnumerable<Report>> GetReportsByUserAndFilters(int userId, ReportStatus? status, SeverityLevel? severity)
        {
            try
            {
                var query = _dbContext.Reports.AsQueryable();

                query = query.Where(r => r.UserId == userId);

                if (status.HasValue)
                {
                    query = query.Where(r => r.Status == status.Value);
                }

                if (severity.HasValue)
                {
                    query = query.Where(r => r.Severity == severity.Value);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting reports by user ID {userId} with filters.");
                throw;
            }
        }

        public async Task<IEnumerable<Report>> GetReportsByCityAndFilters(
            int cityId,
            ReportStatus? status,
            SeverityLevel? severity,
            DateTime? startDate,
            DateTime? endDate,
            string userName)
        {
            try
            {
                var query = _dbContext.Reports.AsQueryable();

                query = query.Where(r => r.CityId == cityId);

                if (status.HasValue)
                {
                    query = query.Where(r => r.Status == status.Value);
                }

                if (severity.HasValue)
                {
                    query = query.Where(r => r.Severity == severity.Value);
                }

                if (startDate.HasValue)
                {
                    query = query.Where(r => r.ReportDate >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(r => r.ReportDate <= endDate.Value);
                }

                if (!string.IsNullOrEmpty(userName))
                {
                    query = query.Include(r => r.User)
                                 .Where(r => r.User.UserName.Contains(userName));
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting reports by city ID {cityId} with filters.");
                throw;
            }
        }

        public async Task<Report> UpdateReportStatusAsync(int reportId, ReportStatus newStatus)
        {
            try
            {
                var report = await _dbContext.Reports.FindAsync(reportId);
                if (report == null)
                {
                    return null;
                }

                report.Status = newStatus;
                _dbContext.Entry(report).Property(r => r.Status).IsModified = true;
                await _dbContext.SaveChangesAsync();

                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the status of the report with ID {reportId}.");
                throw;
            }
        }

        public async Task CreateReportAsync(Report report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));

            try
            {
                _dbContext.Reports.Add(report);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the report.");
                throw;
            }
        }

        public async Task UpdateReportAsync(Report report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));

            try
            {
                _dbContext.Reports.Update(report);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the report with ID {report.Id}.");
                throw;
            }
        }

        public async Task DeleteReportAsync(int reportId)
        {
            try
            {
                var report = await _dbContext.Reports.FindAsync(reportId);
                if (report != null)
                {
                    _dbContext.Reports.Remove(report);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the report with ID {reportId}.");
                throw;
            }
        }

        public async Task CreateReportImageAsync(ReportImage reportImage)
        {
            try
            {
                _dbContext.ReportImage.Add(reportImage);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the report image.");
                throw;
            }
        }

        public async Task DeleteReportImageAsync(int reportId)
        {
            try
            {
                List<ReportImage> imagesToDelete = await _dbContext.ReportImage
                    .Where(image => image.ReportId == reportId)
                    .ToListAsync();

                foreach (var image in imagesToDelete)
                {
                    _dbContext.ReportImage.Remove(image);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting images for report ID {reportId}.");
                throw;
            }
        }

        public async Task<List<ReportImage>> GetAllImagesAsync()
        {
            try
            {
                return await _dbContext.ReportImage.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all report images.");
                throw;
            }
        }

        public async Task<List<ReportImage>> GetImagesByReportIdAsync(int reportId)
        {
            try
            {
                return await _dbContext.ReportImage
                    .Where(ri => ri.ReportId == reportId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting images for report ID {reportId}.");
                throw;
            }
        }

        public async Task<bool> ReportHasImagesAsync(int reportId)
        {
            try
            {
                return await _dbContext.ReportImage
                    .AnyAsync(ri => ri.ReportId == reportId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while checking if report ID {reportId} has images.");
                throw;
            }
        }
    }
}
