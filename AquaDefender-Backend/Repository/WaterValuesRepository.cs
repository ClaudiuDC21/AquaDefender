using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Data;
using AquaDefender_Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AquaDefender_Backend.Repository.Interfaces
{
    public class WaterValuesRepository : IWaterValuesRepository
    {
        private readonly AquaDefenderDataContext _dbContext;
        private readonly ILogger<WaterValuesRepository> _logger;

        public WaterValuesRepository(AquaDefenderDataContext dbContext, ILogger<WaterValuesRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<WaterValues> GetWaterValuesByIdAsync(int waterValuesId)
        {
            try
            {
                return await _dbContext.WaterValues.FindAsync(waterValuesId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the water values with ID {waterValuesId}.");
                throw;
            }
        }

        public async Task<List<WaterValues>> GetAllWaterValuesByWaterInfoIdAsync(int waterInfoId)
        {
            try
            {
                return await _dbContext.WaterValues.Where(w => w.IdWaterInfo == waterInfoId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting all water values for water info ID {waterInfoId}.");
                throw;
            }
        }

        public async Task<List<WaterValues>> GetAllWaterValuesAsync() // Implementarea pentru Get All
        {
            try
            {
                return await _dbContext.WaterValues.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all water values.");
                throw;
            }
        }
        
        public async Task CreateWaterValuesAsync(WaterValues waterValues)
        {
            if (waterValues == null)
                throw new ArgumentNullException(nameof(waterValues));

            try
            {
                _dbContext.WaterValues.Add(waterValues);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the water values.");
                throw;
            }
        }

        public async Task UpdateWaterValuesAsync(WaterValues waterValues)
        {
            if (waterValues == null)
                throw new ArgumentNullException(nameof(waterValues));

            try
            {
                _dbContext.WaterValues.Update(waterValues);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the water values with ID {waterValues.Id}.");
                throw;
            }
        }

        public async Task DeleteWaterValuesAsync(int waterValuesId)
        {
            try
            {
                var waterValues = await _dbContext.WaterValues.FindAsync(waterValuesId);
                if (waterValues != null)
                {
                    _dbContext.WaterValues.Remove(waterValues);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the water values with ID {waterValuesId}.");
                throw;
            }
        }
    }
}
