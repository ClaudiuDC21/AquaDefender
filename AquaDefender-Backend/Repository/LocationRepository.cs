using AquaDefender_Backend.Data;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaDefender_Backend.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly AquaDefenderDataContext _dbContext;

        public LocationRepository(AquaDefenderDataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<City> GetCityByIdAsync(int cityId)
        {
            return await _dbContext.City.Include(c => c.County).FirstOrDefaultAsync(c => c.Id == cityId);
        }

        public async Task<List<City>> GetAllCitiesAsync()
        {
            return await _dbContext.City.Include(c => c.County).ToListAsync();
        }

        public async Task<County> GetCountyByIdAsync(int countyId)
        {
            return await _dbContext.County.Include(c => c.Cities).FirstOrDefaultAsync(c => c.Id == countyId);
        }

        public async Task<List<County>> GetAllCountiesAsync()
        {
            return await _dbContext.County.ToListAsync();
        }

        // Noua metodă pentru a obține toate orașele dintr-un anumit județ
        public async Task<List<City>> GetAllCitiesByCountyIdAsync(int countyId)
        {
            return await _dbContext.City
                                    .Where(c => c.CountyId == countyId)
                                    .Include(c => c.County)
                                    .ToListAsync();
        }

        public async Task<City> GetCityByNameAsync(string cityName)
        {
            return await _dbContext.City.FirstOrDefaultAsync(c => c.Name == cityName);
        }

        public async Task<County> GetCountyByNameAsync(string countyName)
        {
            return await _dbContext.County.FirstOrDefaultAsync(c => c.Name == countyName);
        }

    }
}
