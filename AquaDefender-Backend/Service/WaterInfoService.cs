using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using AquaDefender_Backend.Services.Interfaces;

namespace AquaDefender_Backend.Services
{
    public class WaterInfoService : IWaterInfoService
    {
        private readonly IWaterInfoRepository _waterInfoRepository;

        public WaterInfoService(IWaterInfoRepository waterInfoRepository)
        {
            _waterInfoRepository = waterInfoRepository ?? throw new ArgumentNullException(nameof(waterInfoRepository));
        }

        public async Task<WaterInfo> GetWaterInfoByIdAsync(int waterInfoId)
        {
            if (waterInfoId <= 0)
            {
                throw new ArgumentException("Id-ul informației despre apă trebuie să fie un număr întreg pozitiv.", nameof(waterInfoId));
            }

            var waterInfo = await _waterInfoRepository.GetWaterInfoByIdAsync(waterInfoId);
            if (waterInfo == null)
            {
                throw new ArgumentException($"Informația despre apă cu id-ul {waterInfoId} nu există.");
            }

            return waterInfo;
        }

        public async Task<List<WaterInfo>> GetAllWaterInfosAsync()
        {
            return await _waterInfoRepository.GetAllWaterInfosAsync();
        }

        public async Task<List<WaterInfo>> GetAllWaterInfosByCityIdAsync(int cityId)
        {
            if (cityId <= 0)
            {
                throw new ArgumentException("Id-ul orașului trebuie să fie un număr întreg pozitiv.", nameof(cityId));
            }

            return await _waterInfoRepository.GetAllWaterInfosByCityIdAsync(cityId);
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

            var waterInfo = await _waterInfoRepository.GetReportByDateAndCityAsync(date, cityId);

            return waterInfo;
        }

        public async Task CreateWaterInfoAsync(WaterInfo waterInfo)
        {
            if (waterInfo == null)
            {
                throw new ArgumentNullException(nameof(waterInfo), "Obiectul WaterInfo nu poate fi nul.");
            }

             var existingReport = await _waterInfoRepository.GetReportByDateAndCityAsync(waterInfo.DateReported, waterInfo.CityId);
            if (existingReport != null)
            {
                throw new InvalidOperationException($"Există deja un raport pentru orașul cu ID-ul {waterInfo.CityId} la data de {waterInfo.DateReported.ToShortDateString()}.");
            }

            await _waterInfoRepository.CreateWaterInfoAsync(waterInfo);
        }


        public async Task UpdateWaterInfoAsync(WaterInfo waterInfo)
        {
            if (waterInfo == null)
            {
                throw new ArgumentNullException(nameof(waterInfo), "Obiectul WaterInfo nu poate fi nul.");
            }

            var existingWaterInfo = await _waterInfoRepository.GetWaterInfoByIdAsync(waterInfo.Id);
            if (existingWaterInfo == null)
            {
                throw new ArgumentException($"Informația despre apă cu id-ul {waterInfo.Id} nu există.");
            }

            await _waterInfoRepository.UpdateWaterInfoAsync(waterInfo);
        }

        public async Task DeleteWaterInfoAsync(int waterInfoId)
        {
            if (waterInfoId <= 0)
            {
                throw new ArgumentException("Id-ul informației despre apă trebuie să fie un număr întreg pozitiv.", nameof(waterInfoId));
            }

            var waterInfo = await _waterInfoRepository.GetWaterInfoByIdAsync(waterInfoId);
            if (waterInfo == null)
            {
                throw new ArgumentException($"Informația despre apă cu id-ul {waterInfoId} nu există.");
            }

            await _waterInfoRepository.DeleteWaterInfoAsync(waterInfoId);
        }

    }
}
