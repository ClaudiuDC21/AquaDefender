using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Data;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AquaDefender_Backend.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly AquaDefenderDataContext _dbContext;

        public ReportRepository(AquaDefenderDataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Report> GetReportByIdAsync(int reportId)
        {
            return await _dbContext.Reports.FindAsync(reportId);
        }

        public async Task<List<Report>> GetAllReportsAsync()
        {
            return await _dbContext.Reports.ToListAsync();
        }

        public async Task<List<Report>> GetReportsByStatusAsync(ReportStatus status)
        {
            return await _dbContext.Reports
                .Where(report => report.Status == status)
                .ToListAsync();
        }

        public async Task<int> GetReportCountByUserIdAsync(int userId)
        {
            return await _dbContext.Reports
                                 .CountAsync(r => r.UserId == userId);
        }

        public async Task<int> GetNewReportCountByUserIdAsync(int userId)
        {
            return await _dbContext.Reports
                                 .CountAsync(r => r.UserId == userId && r.Status == ReportStatus.New);
        }

        public async Task<int> GetInProgressReportCountByUserIdAsync(int userId)
        {
            return await _dbContext.Reports
                                 .CountAsync(r => r.UserId == userId && r.Status == ReportStatus.InProgress);
        }

        public async Task<int> GetResolvedReportCountByUserIdAsync(int userId)
        {
            return await _dbContext.Reports
                                 .CountAsync(r => r.UserId == userId && r.Status == ReportStatus.Resolved);
        }

        public async Task<int> GetReportCountByCityIdAsync(int cityId)
        {
            return await _dbContext.Reports.CountAsync(r => r.CityId == cityId);
        }

        public async Task<int> GetReportCountByCityIdAndStatusAsync(int cityId, ReportStatus status)
        {
            return await _dbContext.Reports.CountAsync(r => r.CityId == cityId && r.Status == status);
        }

        public async Task<int> GetNewReportCountByCityIdAsync(int cityId)
        {
            return await _dbContext.Reports
                .CountAsync(r => r.CityId == cityId && r.Status == ReportStatus.New);
        }

        public async Task<int> GetReportCountByCityIdAndSeverityAsync(int cityId, SeverityLevel severity)
        {
            return await _dbContext.Reports.CountAsync(r => r.CityId == cityId && r.Severity == severity);
        }

        public async Task<List<Report>> GetRandomReportsByStatus(ReportStatus status, int count)
        {
            return await _dbContext.Reports
                                 .Where(r => r.Status == status)
                                 .OrderBy(r => Guid.NewGuid())
                                 .Take(count)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetReportsByUserAndFilters(int userId, ReportStatus? status, SeverityLevel? severity)
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

        public async Task<IEnumerable<Report>> GetReportsByCityAndFilters(
            int cityId,
            ReportStatus? status,
            SeverityLevel? severity,
            DateTime? startDate,
            DateTime? endDate,
            string userName)
        {
            var query = _dbContext.Reports.AsQueryable();

            // Filtru după ID-ul orașului
            query = query.Where(r => r.CityId == cityId);

            // Filtru după status, dacă este furnizat
            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status.Value);
            }

            // Filtru după severitate, dacă este furnizat
            if (severity.HasValue)
            {
                query = query.Where(r => r.Severity == severity.Value);
            }

            // Filtru după data de început, dacă este furnizată
            if (startDate.HasValue)
            {
                query = query.Where(r => r.ReportDate >= startDate.Value);
            }

            // Filtru după data de sfârșit, dacă este furnizată
            if (endDate.HasValue)
            {
                query = query.Where(r => r.ReportDate <= endDate.Value);
            }

            // Include tabela Users pentru a putea efectua filtrarea după numele utilizatorilor, dacă este furnizat
            if (!string.IsNullOrEmpty(userName))
            {
                // Presupunem că avem acces la tabela Users printr-o relație definită în modelul Report
                query = query.Include(r => r.User)
                             .Where(r => r.User.UserName.Contains(userName));
            }

            return await query.ToListAsync();
        }

        public async Task<Report> UpdateReportStatusAsync(int reportId, ReportStatus newStatus)
        {
            var report = await _dbContext.Reports.FindAsync(reportId);
            if (report == null)
            {
                return null;
            }

            // Actualizează doar statusul raportului
            report.Status = newStatus;

            // Marchează doar proprietatea 'Status' ca fiind modificată
            _dbContext.Entry(report).Property(r => r.Status).IsModified = true;

            // Salvează modificările în baza de date
            await _dbContext.SaveChangesAsync();

            return report;
        }



        public async Task CreateReportAsync(Report report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));

            _dbContext.Reports.Add(report);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateReportAsync(Report report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));

            _dbContext.Reports.Update(report);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteReportAsync(int reportId)
        {
            var report = await _dbContext.Reports.FindAsync(reportId);
            if (report != null)
            {
                _dbContext.Reports.Remove(report);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task CreateReportImageAsync(ReportImage reportImage)
        {
            _dbContext.ReportImage.Add(reportImage);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteReportImageAsync(int reportId)
        {
            // Find images with the given reportId
            List<ReportImage> imagesToDelete = await _dbContext.ReportImage
                .Where(image => image.ReportId == reportId)
                .ToListAsync();

            foreach (var image in imagesToDelete)
            {
                // Remove each image individually
                _dbContext.ReportImage.Remove(image);
            }

            // Save changes after removing all images
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ReportImage>> GetAllImagesAsync()
        {
            return await _dbContext.ReportImage.ToListAsync();
        }

        public async Task<List<ReportImage>> GetImagesByReportIdAsync(int reportId)
        {
            return await _dbContext.ReportImage
                .Where(ri => ri.ReportId == reportId)
                .ToListAsync();
        }

        public async Task<bool> ReportHasImagesAsync(int reportId)
        {
            return await _dbContext.ReportImage
                .AnyAsync(ri => ri.ReportId == reportId);
        }
    }
}