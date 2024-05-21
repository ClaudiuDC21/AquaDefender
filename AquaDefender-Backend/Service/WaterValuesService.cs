using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using AquaDefender_Backend.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaDefender_Backend.Services
{
    public class WaterValuesService : IWaterValuesService
    {
        private readonly IWaterValuesRepository _waterValuesRepository;

        public WaterValuesService(IWaterValuesRepository waterValuesRepository)
        {
            _waterValuesRepository = waterValuesRepository ?? throw new ArgumentNullException(nameof(waterValuesRepository));
        }

        public async Task<WaterValues> GetWaterValuesByIdAsync(int waterValuesId)
        {
            if (waterValuesId <= 0)
            {
                throw new ArgumentException("Id-ul valorilor de apă trebuie să fie un număr întreg pozitiv.", nameof(waterValuesId));
            }

            var waterValues = await _waterValuesRepository.GetWaterValuesByIdAsync(waterValuesId);
            if (waterValues == null)
            {
                throw new ArgumentException($"Valorile de apă cu id-ul {waterValuesId} nu există.");
            }

            return waterValues;
        }

        public async Task<List<WaterValues>> GetAllWaterValuesByWaterInfoIdAsync(int waterInfoId)
        {
            if (waterInfoId <= 0)
            {
                throw new ArgumentException("Id-ul informației despre apă trebuie să fie un număr întreg pozitiv.", nameof(waterInfoId));
            }

            return await _waterValuesRepository.GetAllWaterValuesByWaterInfoIdAsync(waterInfoId);
        }

        public async Task<List<WaterValues>> GetAllWaterValuesAsync()
        {
            return await _waterValuesRepository.GetAllWaterValuesAsync();
        }

        public async Task CreateWaterValuesAsync(WaterValues waterValues)
        {
            if (waterValues == null)
            {
                throw new ArgumentNullException(nameof(waterValues), "Obiectul WaterValues nu poate fi nul.");
            }

            await _waterValuesRepository.CreateWaterValuesAsync(waterValues);
        }

        public async Task UpdateWaterValuesAsync(WaterValues waterValues)
        {
            if (waterValues == null)
            {
                throw new ArgumentNullException(nameof(waterValues), "Obiectul WaterValues nu poate fi nul.");
            }

            // Verifică dacă valorile de apă există în baza de date
            var existingWaterValues = await _waterValuesRepository.GetWaterValuesByIdAsync(waterValues.Id);
            if (existingWaterValues == null)
            {
                throw new ArgumentException($"Valorile de apă cu id-ul {waterValues.Id} nu există.");
            }

            await _waterValuesRepository.UpdateWaterValuesAsync(waterValues);
        }

        public async Task DeleteWaterValuesAsync(int waterValuesId)
        {
            if (waterValuesId <= 0)
            {
                throw new ArgumentException("Id-ul valorilor de apă trebuie să fie un număr întreg pozitiv.", nameof(waterValuesId));
            }

            // Verifică dacă valorile de apă există în baza de date
            var waterValues = await _waterValuesRepository.GetWaterValuesByIdAsync(waterValuesId);
            if (waterValues == null)
            {
                throw new ArgumentException($"Valorile de apă cu id-ul {waterValuesId} nu există.");
            }

            await _waterValuesRepository.DeleteWaterValuesAsync(waterValuesId);
        }

    }
}
