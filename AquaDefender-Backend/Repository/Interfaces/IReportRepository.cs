using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Domain;

namespace AquaDefender_Backend.Repository.Interfaces
{
    public interface IReportRepository
    {
        Task<Report> GetReportByIdAsync(int reportId);
        Task<List<Report>> GetAllReportsAsync();
        Task<List<Report>> GetReportsByStatusAsync(ReportStatus status);
        Task<int> GetReportCountByUserIdAsync(int userId);
        Task<int> GetNewReportCountByUserIdAsync(int userId);
        Task<int> GetInProgressReportCountByUserIdAsync(int userId);
        Task<int> GetResolvedReportCountByUserIdAsync(int userId);
        Task<List<Report>> GetRandomReportsByStatus(ReportStatus status, int count);
        Task<int> GetReportCountByCityIdAsync(int cityId);
        Task<int> GetReportCountByCityIdAndStatusAsync(int cityId, ReportStatus status);
        Task<int> GetReportCountByCityIdAndSeverityAsync(int cityId, SeverityLevel severity);
        Task<int> GetNewReportCountByCityIdAsync(int cityId);
        Task<IEnumerable<Report>> GetReportsByUserAndFilters(int userId, ReportStatus? status, SeverityLevel? severity);
        Task<IEnumerable<Report>> GetReportsByCityAndFilters(
            int cityId,
            ReportStatus? status,
            SeverityLevel? severity,
            DateTime? startDate,
            DateTime? endDate,
            string userName);
        Task<Report> UpdateReportStatusAsync(int reportId, ReportStatus newStatus);
        Task CreateReportAsync(Report report);
        Task UpdateReportAsync(Report report);
        Task DeleteReportAsync(int reportId);
        Task CreateReportImageAsync(ReportImage reportImage);
        Task DeleteReportImageAsync(int reportId);
        Task<List<ReportImage>> GetAllImagesAsync();
        Task<List<ReportImage>> GetImagesByReportIdAsync(int reportId);
        Task<bool> ReportHasImagesAsync(int reportId);
    }
}