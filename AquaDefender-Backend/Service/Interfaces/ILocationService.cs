using AquaDefender_Backend.Domain;

namespace AquaDefender_Backend.Service.Interfaces
{
    public interface ILocationService
    {
        Task<County> GetCountyByIdAsync(int countyId);
        Task<List<County>> GetAllCountiesAsync();
        Task<City> GetCityByIdAsync(int cityId);
        Task<List<City>> GetAllCitiesAsync();
        Task<List<City>> GetAllCitiesByCountyIdAsync(int countyId);
        Task<City> GetCityByNameAsync(string cityName);
        Task<County> GetCountyByNameAsync(string countyName);
        Task<string> GetCityEmailByIdAsync(int cityId);
        Task<string> GetCountyEmailByIdAsync(int countyId);
    }
}
