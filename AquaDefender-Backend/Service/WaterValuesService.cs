using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Repository.Interfaces;
using AquaDefender_Backend.Service.Interfaces;

namespace AquaDefender_Backend.Services
{
    public class WaterValuesService : IWaterValuesService
    {
        private readonly IWaterValuesRepository _waterValuesRepository;
        private readonly ILogger<WaterValuesService> _logger;

        public WaterValuesService(IWaterValuesRepository waterValuesRepository, ILogger<WaterValuesService> logger)
        {
            _waterValuesRepository = waterValuesRepository ?? throw new ArgumentNullException(nameof(waterValuesRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<WaterValues> GetWaterValuesByIdAsync(int waterValuesId)
        {
            if (waterValuesId <= 0)
            {
                throw new ArgumentException("Id-ul valorilor de apă trebuie să fie un număr întreg pozitiv.", nameof(waterValuesId));
            }

            try
            {
                var waterValues = await _waterValuesRepository.GetWaterValuesByIdAsync(waterValuesId);
                if (waterValues == null)
                {
                    throw new ArgumentException($"Valorile de apă cu id-ul {waterValuesId} nu există.");
                }

                return waterValues;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the water values with ID {waterValuesId}.");
                throw;
            }
        }

        public async Task<List<WaterValues>> GetAllWaterValuesByWaterInfoIdAsync(int waterInfoId)
        {
            if (waterInfoId <= 0)
            {
                throw new ArgumentException("Id-ul informației despre apă trebuie să fie un număr întreg pozitiv.", nameof(waterInfoId));
            }

            try
            {
                return await _waterValuesRepository.GetAllWaterValuesByWaterInfoIdAsync(waterInfoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting all water values for water info ID {waterInfoId}.");
                throw;
            }
        }

        public async Task<List<WaterValues>> GetAllWaterValuesAsync()
        {
            try
            {
                return await _waterValuesRepository.GetAllWaterValuesAsync();
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
            {
                throw new ArgumentNullException(nameof(waterValues), "Obiectul WaterValues nu poate fi nul.");
            }

            try
            {
                await _waterValuesRepository.CreateWaterValuesAsync(waterValues);
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
            {
                throw new ArgumentNullException(nameof(waterValues), "Obiectul WaterValues nu poate fi nul.");
            }

            try
            {
                var existingWaterValues = await _waterValuesRepository.GetWaterValuesByIdAsync(waterValues.Id);
                if (existingWaterValues == null)
                {
                    throw new ArgumentException($"Valorile de apă cu id-ul {waterValues.Id} nu există.");
                }

                await _waterValuesRepository.UpdateWaterValuesAsync(waterValues);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the water values with ID {waterValues.Id}.");
                throw;
            }
        }

        public async Task DeleteWaterValuesAsync(int waterValuesId)
        {
            if (waterValuesId <= 0)
            {
                throw new ArgumentException("Id-ul valorilor de apă trebuie să fie un număr întreg pozitiv.", nameof(waterValuesId));
            }

            try
            {
                var waterValues = await _waterValuesRepository.GetWaterValuesByIdAsync(waterValuesId);
                if (waterValues == null)
                {
                    throw new ArgumentException($"Valorile de apă cu id-ul {waterValuesId} nu există.");
                }

                await _waterValuesRepository.DeleteWaterValuesAsync(waterValuesId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the water values with ID {waterValuesId}.");
                throw;
            }
        }
    }
}
