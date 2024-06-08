using AquaDefender_Backend.Domain;

namespace AquaDefender_Backend.Service.Interfaces
{
    public interface IWaterValuesService
    {
        Task<WaterValues> GetWaterValuesByIdAsync(int waterValuesId);
        Task<List<WaterValues>> GetAllWaterValuesByWaterInfoIdAsync(int waterInfoId);
        Task<List<WaterValues>> GetAllWaterValuesAsync();
        Task CreateWaterValuesAsync(WaterValues waterValues);
        Task UpdateWaterValuesAsync(WaterValues waterValues);
        Task DeleteWaterValuesAsync(int waterValuesId);
    }
}