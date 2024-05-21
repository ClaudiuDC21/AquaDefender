using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.DTOs;

namespace AquaDefender_Backend.Service.Interfaces
{
    public interface IReportService
    {
        Task<Report> GetReportByIdAsync(int reportId);
        Task<List<Report>> GetAllReportsAsync();
        Task<List<Report>> GetReportsByStatusAsync(ReportStatus status);
        Task<int> GetReportCountByUserIdAsync(int userId);
        Task<int> GetNewReportCountByUserIdAsync(int userId);
        Task<int> GetInProgressReportCountByUserIdAsync(int userId);
        Task<int> GetResolvedReportCountByUserIdAsync(int userId);
        Task<List<Report>> GetRandomNewReports();
        Task<List<Report>> GetRandomInProgressReports();
        Task<List<Report>> GetRandomCompletedReports();
        Task<ReportStatisticsDto> GetReportStatisticsByCityIdAsync(int cityId);
        Task<int> GetNewReportCountByCityIdAsync(int cityId);
        Task<IEnumerable<Report>> GetFilteredReportsByUserId(int userId, ReportStatus? status, SeverityLevel? severity);
        Task<IEnumerable<Report>> GetFilteredReportsByCityId(int cityId, ReportStatus? status, SeverityLevel? severity, DateTime? startDate, DateTime? endDate, string userName);
        Task<Report> UpdateReportStatusAsync(int reportId, ReportStatus newStatus);
        Task CreateReportAsync(Report report, List<string>? images);
        Task UpdateReportAsync(Report report, List<string> savedImagePaths);
        Task DeleteReportAsync(int reportId);
        Task<List<ReportImage>> GetAllImagesAsync();
        Task<List<ReportImage>> GetImagesByReportIdAsync(int reportId);
        Task<bool> ReportHasImagesAsync(int reportId);
    }
}