using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using AquaDefender_Backend.Service.Interfaces;

namespace AquaDefender_Backend.Service
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
        }

        public async Task<City> GetCityByIdAsync(int cityId)
        {
            if (cityId <= 0)
            {
                throw new ArgumentException("Id-ul orașului trebuie să fie un număr întreg pozitiv.");
            }

            var city = await _locationRepository.GetCityByIdAsync(cityId);
            if (city == null)
            {
                throw new ArgumentException($"Orașul cu id-ul {cityId} nu există.");
            }

            return city;
        }

        public async Task<List<City>> GetAllCitiesAsync()
        {
            return await _locationRepository.GetAllCitiesAsync();
        }

        public async Task<County> GetCountyByIdAsync(int countyId)
        {
            if (countyId <= 0)
            {
                throw new ArgumentException("Id-ul județului trebuie să fie un număr întreg pozitiv.");
            }

            var county = await _locationRepository.GetCountyByIdAsync(countyId);
            if (county == null)
            {
                throw new ArgumentException($"Județul cu id-ul {countyId} nu există.");
            }

            return county;
        }

        public async Task<List<County>> GetAllCountiesAsync()
        {
            return await _locationRepository.GetAllCountiesAsync();
        }

        public async Task<List<City>> GetAllCitiesByCountyIdAsync(int countyId)
        {
            if (countyId <= 0)
            {
                throw new ArgumentException("Id-ul județului trebuie să fie un număr întreg pozitiv.");
            }

            var county = await _locationRepository.GetCountyByIdAsync(countyId);
            if (county == null)
            {
                throw new ArgumentException($"Județul cu id-ul {countyId} nu există.");
            }

            return await _locationRepository.GetAllCitiesByCountyIdAsync(countyId);
        }

        public async Task<City> GetCityByNameAsync(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                throw new ArgumentException("Numele orașului nu trebuie să fie nul sau gol.");
            }

            if (cityName.Length > 100)
            {
                throw new ArgumentException("Numele orașului nu trebuie să aibă mai mult de 100 de caractere.");
            }

            var city = await _locationRepository.GetCityByNameAsync(cityName);
            if (city == null)
            {
                throw new ArgumentException($"Orașul cu numele {cityName} nu există.");
            }

            return city;
        }

        public async Task<County> GetCountyByNameAsync(string countyName)
        {
            if (string.IsNullOrEmpty(countyName))
            {
                throw new ArgumentException("Numele județului nu trebuie să fie nul sau gol.");
            }

            if (countyName.Length > 100)
            {
                throw new ArgumentException("Numele județului nu trebuie să aibă mai mult de 100 de caractere.");
            }

            var county = await _locationRepository.GetCountyByNameAsync(countyName);
            if (county == null)
            {
                throw new ArgumentException($"Județul cu numele {countyName} nu există.");
            }

            return county;
        }

    }
}
