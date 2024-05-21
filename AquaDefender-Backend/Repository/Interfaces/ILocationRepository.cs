using System.Collections.Generic;
using System.Threading.Tasks;
using AquaDefender_Backend.Domain;

namespace AquaDefender_Backend.Repository.Interfaces
{
    public interface ILocationRepository
    {
        Task<County> GetCountyByIdAsync(int countyId);
        Task<List<County>> GetAllCountiesAsync();
        Task<City> GetCityByIdAsync(int cityId);
        Task<List<City>> GetAllCitiesAsync();
        Task<List<City>> GetAllCitiesByCountyIdAsync(int countyId);
        Task<City> GetCityByNameAsync(string cityName);
        Task<County> GetCountyByNameAsync(string countyName);
    }
}
