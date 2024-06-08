using AquaDefender_Backend.Data;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AquaDefender_Backend.Repositories
{
    public class WaterInfoRepository : IWaterInfoRepository
    {
        private readonly AquaDefenderDataContext _dbContext;
        private readonly ILogger<WaterInfoRepository> _logger;

        public WaterInfoRepository(AquaDefenderDataContext dbContext, ILogger<WaterInfoRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<WaterInfo> GetWaterInfoByIdAsync(int waterInfoId)
        {
            try
            {
                return await _dbContext.WaterInfos.FindAsync(waterInfoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the water info with ID {waterInfoId}.");
                throw;
            }
        }

        public async Task<List<WaterInfo>> GetAllWaterInfosAsync()
        {
            try
            {
                return await _dbContext.WaterInfos.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all water infos.");
                throw;
            }
        }

        public async Task<List<WaterInfo>> GetAllWaterInfosByCityIdAsync(int cityId)
        {
            try
            {
                return await _dbContext.WaterInfos
                                     .Where(waterInfo => waterInfo.CityId == cityId)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting all water infos for city ID {cityId}.");
                throw;
            }
        }

        public async Task<WaterInfo> GetReportByDateAndCityAsync(DateTime date, int cityId)
        {
            try
            {
                return await _dbContext.WaterInfos
                                     .FirstOrDefaultAsync(wi => wi.DateReported.Date == date.Date
                                                                && wi.CityId == cityId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the report for date {date.ToShortDateString()} and city ID {cityId}.");
                throw;
            }
        }

        public async Task CreateWaterInfoAsync(WaterInfo waterInfo)
        {
            if (waterInfo == null)
                throw new ArgumentNullException(nameof(waterInfo));

            try
            {
                _dbContext.WaterInfos.Add(waterInfo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the water info.");
                throw;
            }
        }

        public async Task UpdateWaterInfoAsync(WaterInfo waterInfo)
        {
            if (waterInfo == null)
                throw new ArgumentNullException(nameof(waterInfo));

            try
            {
                _dbContext.WaterInfos.Update(waterInfo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the water info with ID {waterInfo.Id}.");
                throw;
            }
        }

        public async Task DeleteWaterInfoAsync(int waterInfoId)
        {
            try
            {
                var waterInfo = await _dbContext.WaterInfos.FindAsync(waterInfoId);
                if (waterInfo != null)
                {
                    _dbContext.WaterInfos.Remove(waterInfo);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the water info with ID {waterInfoId}.");
                throw;
            }
        }
    }
}
