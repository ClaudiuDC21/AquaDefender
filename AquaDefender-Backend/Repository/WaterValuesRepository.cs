using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Data;
using AquaDefender_Backend.Domain;
using Microsoft.EntityFrameworkCore;

namespace AquaDefender_Backend.Repository.Interfaces
{
    public class WaterValuesRepository : IWaterValuesRepository
    {
        private readonly AquaDefenderDataContext _dbContext;

        public WaterValuesRepository(AquaDefenderDataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<WaterValues> GetWaterValuesByIdAsync(int waterValuesId)
        {
            return await _dbContext.WaterValues.FindAsync(waterValuesId);
        }

        public async Task<List<WaterValues>> GetAllWaterValuesByWaterInfoIdAsync(int waterInfoId)
        {
            return await _dbContext.WaterValues.Where(w => w.IdWaterInfo == waterInfoId).ToListAsync();
        }

         public async Task<List<WaterValues>> GetAllWaterValuesAsync() // Implementarea pentru Get All
        {
            return await _dbContext.WaterValues.ToListAsync();
        }
        
        public async Task CreateWaterValuesAsync(WaterValues waterValues)
        {
            if (waterValues == null)
                throw new ArgumentNullException(nameof(waterValues));

            _dbContext.WaterValues.Add(waterValues);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateWaterValuesAsync(WaterValues waterValues)
        {
            if (waterValues == null)
                throw new ArgumentNullException(nameof(waterValues));

            _dbContext.WaterValues.Update(waterValues);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteWaterValuesAsync(int waterValuesId)
        {
            var waterValues = await _dbContext.WaterValues.FindAsync(waterValuesId);
            if (waterValues != null)
            {
                _dbContext.WaterValues.Remove(waterValues);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}