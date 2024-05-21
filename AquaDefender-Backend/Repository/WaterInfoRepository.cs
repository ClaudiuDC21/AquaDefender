using AquaDefender_Backend.Data;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaDefender_Backend.Repositories
{
    public class WaterInfoRepository : IWaterInfoRepository
    {
        private readonly AquaDefenderDataContext _dbContext;

        public WaterInfoRepository(AquaDefenderDataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<WaterInfo> GetWaterInfoByIdAsync(int waterInfoId)
        {
            return await _dbContext.WaterInfos.FindAsync(waterInfoId);
        }

        public async Task<List<WaterInfo>> GetAllWaterInfosAsync()
        {
            return await _dbContext.WaterInfos.ToListAsync();
        }

        public async Task<List<WaterInfo>> GetAllWaterInfosByCityIdAsync(int cityId)
        {
            return await _dbContext.WaterInfos
                                 .Where(waterInfo => waterInfo.CityId == cityId)
                                 .ToListAsync();
        }

        public async Task<WaterInfo> GetReportByDateAndCityAsync(DateTime date, int cityId)
        {
            return await _dbContext.WaterInfos
                                 .FirstOrDefaultAsync(wi => wi.DateReported.Date == date.Date
                                                            && wi.CityId == cityId);
        }

        public async Task CreateWaterInfoAsync(WaterInfo waterInfo)
        {
            if (waterInfo == null)
                throw new ArgumentNullException(nameof(waterInfo));

            _dbContext.WaterInfos.Add(waterInfo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateWaterInfoAsync(WaterInfo waterInfo)
        {
            if (waterInfo == null)
                throw new ArgumentNullException(nameof(waterInfo));

            _dbContext.WaterInfos.Update(waterInfo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteWaterInfoAsync(int waterInfoId)
        {
            var waterInfo = await _dbContext.WaterInfos.FindAsync(waterInfoId);
            if (waterInfo != null)
            {
                _dbContext.WaterInfos.Remove(waterInfo);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
