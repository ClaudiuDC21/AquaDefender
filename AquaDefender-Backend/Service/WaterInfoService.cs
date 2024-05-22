using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using AquaDefender_Backend.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace AquaDefender_Backend.Services
{
    public class WaterInfoService : IWaterInfoService
    {
        private readonly IWaterInfoRepository _waterInfoRepository;
        private readonly ILogger<WaterInfoService> _logger;

        public WaterInfoService(IWaterInfoRepository waterInfoRepository, ILogger<WaterInfoService> logger)
        {
            _waterInfoRepository = waterInfoRepository ?? throw new ArgumentNullException(nameof(waterInfoRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<WaterInfo> GetWaterInfoByIdAsync(int waterInfoId)
        {
            if (waterInfoId <= 0)
            {
                throw new ArgumentException("Id-ul informației despre apă trebuie să fie un număr întreg pozitiv.", nameof(waterInfoId));
            }

            try
            {
                var waterInfo = await _waterInfoRepository.GetWaterInfoByIdAsync(waterInfoId);
                if (waterInfo == null)
                {
                    throw new ArgumentException($"Informația despre apă cu id-ul {waterInfoId} nu există.");
                }

                return waterInfo;
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
                return await _waterInfoRepository.GetAllWaterInfosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all water infos.");
                throw;
            }
        }

        public async Task<List<WaterInfo>> GetAllWaterInfosByCityIdAsync(int cityId)
        {
            if (cityId <= 0)
            {
                throw new ArgumentException("Id-ul orașului trebuie să fie un număr întreg pozitiv.", nameof(cityId));
            }

            try
            {
                return await _waterInfoRepository.GetAllWaterInfosByCityIdAsync(cityId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting water infos for city with ID {cityId}.");
                throw;
            }
        }

        public async Task<WaterInfo> GetReportByDateAndCityAsync(DateTime date, int cityId)
        {
            if (cityId <= 0)
            {
                throw new ArgumentException("Id-ul orașului trebuie să fie un număr întreg pozitiv.", nameof(cityId));
            }

            if (date == default)
            {
                throw new ArgumentException("Data furnizată este invalidă.", nameof(date));
            }

            try
            {
                var waterInfo = await _waterInfoRepository.GetReportByDateAndCityAsync(date, cityId);
                return waterInfo;
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
            {
                throw new ArgumentNullException(nameof(waterInfo), "Obiectul WaterInfo nu poate fi nul.");
            }

            try
            {
                var existingReport = await _waterInfoRepository.GetReportByDateAndCityAsync(waterInfo.DateReported, waterInfo.CityId);
                if (existingReport != null)
                {
                    throw new InvalidOperationException($"Există deja un raport pentru orașul cu ID-ul {waterInfo.CityId} la data de {waterInfo.DateReported.ToShortDateString()}.");
                }

                await _waterInfoRepository.CreateWaterInfoAsync(waterInfo);
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
            {
                throw new ArgumentNullException(nameof(waterInfo), "Obiectul WaterInfo nu poate fi nul.");
            }

            try
            {
                var existingWaterInfo = await _waterInfoRepository.GetWaterInfoByIdAsync(waterInfo.Id);
                if (existingWaterInfo == null)
                {
                    throw new ArgumentException($"Informația despre apă cu id-ul {waterInfo.Id} nu există.");
                }

                await _waterInfoRepository.UpdateWaterInfoAsync(waterInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the water info with ID {waterInfo.Id}.");
                throw;
            }
        }

        public async Task DeleteWaterInfoAsync(int waterInfoId)
        {
            if (waterInfoId <= 0)
            {
                throw new ArgumentException("Id-ul informației despre apă trebuie să fie un număr întreg pozitiv.", nameof(waterInfoId));
            }

            try
            {
                var waterInfo = await _waterInfoRepository.GetWaterInfoByIdAsync(waterInfoId);
                if (waterInfo == null)
                {
                    throw new ArgumentException($"Informația despre apă cu id-ul {waterInfoId} nu există.");
                }

                await _waterInfoRepository.DeleteWaterInfoAsync(waterInfoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the water info with ID {waterInfoId}.");
                throw;
            }
        }
    }
}
