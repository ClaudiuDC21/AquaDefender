using AquaDefender_Backend.Data;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaDefender_Backend.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly AquaDefenderDataContext _dbContext;
        private readonly ILogger<LocationRepository> _logger;

        public LocationRepository(AquaDefenderDataContext dbContext, ILogger<LocationRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<City> GetCityByIdAsync(int cityId)
        {
            try
            {
                return await _dbContext.City.Include(c => c.County).FirstOrDefaultAsync(c => c.Id == cityId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the city with ID {cityId}.");
                throw;
            }
        }

        public async Task<List<City>> GetAllCitiesAsync()
        {
            try
            {
                return await _dbContext.City.Include(c => c.County).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all cities.");
                throw;
            }
        }

        public async Task<County> GetCountyByIdAsync(int countyId)
        {
            try
            {
                return await _dbContext.County.Include(c => c.Cities).FirstOrDefaultAsync(c => c.Id == countyId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the county with ID {countyId}.");
                throw;
            }
        }

        public async Task<List<County>> GetAllCountiesAsync()
        {
            try
            {
                return await _dbContext.County.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all counties.");
                throw;
            }
        }

        public async Task<List<City>> GetAllCitiesByCountyIdAsync(int countyId)
        {
            try
            {
                return await _dbContext.City
                                        .Where(c => c.CountyId == countyId)
                                        .Include(c => c.County)
                                        .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting all cities for county ID {countyId}.");
                throw;
            }
        }

        public async Task<City> GetCityByNameAsync(string cityName)
        {
            try
            {
                return await _dbContext.City.FirstOrDefaultAsync(c => c.Name == cityName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the city with name {cityName}.");
                throw;
            }
        }

        public async Task<County> GetCountyByNameAsync(string countyName)
        {
            try
            {
                return await _dbContext.County.FirstOrDefaultAsync(c => c.Name == countyName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the county with name {countyName}.");
                throw;
            }
        }
    }
}
