using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using AquaDefender_Backend.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace AquaDefender_Backend.Service
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<LocationService> _logger;

        public LocationService(ILocationRepository locationRepository, ILogger<LocationService> logger)
        {
            _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<City> GetCityByIdAsync(int cityId)
        {
            if (cityId <= 0)
            {
                throw new ArgumentException("Id-ul orașului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                var city = await _locationRepository.GetCityByIdAsync(cityId);
                if (city == null)
                {
                    throw new ArgumentException($"Orașul cu id-ul {cityId} nu există.");
                }

                return city;
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
                return await _locationRepository.GetAllCitiesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all cities.");
                throw;
            }
        }

        public async Task<County> GetCountyByIdAsync(int countyId)
        {
            if (countyId <= 0)
            {
                throw new ArgumentException("Id-ul județului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                var county = await _locationRepository.GetCountyByIdAsync(countyId);
                if (county == null)
                {
                    throw new ArgumentException($"Județul cu id-ul {countyId} nu există.");
                }

                return county;
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
                return await _locationRepository.GetAllCountiesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all counties.");
                throw;
            }
        }

        public async Task<List<City>> GetAllCitiesByCountyIdAsync(int countyId)
        {
            if (countyId <= 0)
            {
                throw new ArgumentException("Id-ul județului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                var county = await _locationRepository.GetCountyByIdAsync(countyId);
                if (county == null)
                {
                    throw new ArgumentException($"Județul cu id-ul {countyId} nu există.");
                }

                return await _locationRepository.GetAllCitiesByCountyIdAsync(countyId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting cities by county ID {countyId}.");
                throw;
            }
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

            try
            {
                var city = await _locationRepository.GetCityByNameAsync(cityName);
                if (city == null)
                {
                    throw new ArgumentException($"Orașul cu numele {cityName} nu există.");
                }

                return city;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the city with name {cityName}.");
                throw;
            }
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

            try
            {
                var county = await _locationRepository.GetCountyByNameAsync(countyName);
                if (county == null)
                {
                    throw new ArgumentException($"Județul cu numele {countyName} nu există.");
                }

                return county;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the county with name {countyName}.");
                throw;
            }
        }
    }
}
