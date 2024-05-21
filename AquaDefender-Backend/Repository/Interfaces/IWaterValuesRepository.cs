using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Domain;

namespace AquaDefender_Backend.Repository.Interfaces
{
    public interface IWaterValuesRepository
    {
        Task<WaterValues> GetWaterValuesByIdAsync(int waterValuesId);
        Task<List<WaterValues>> GetAllWaterValuesByWaterInfoIdAsync(int waterInfoId);
        Task<List<WaterValues>> GetAllWaterValuesAsync();
        Task CreateWaterValuesAsync(WaterValues waterValues);
        Task UpdateWaterValuesAsync(WaterValues waterValues);
        Task DeleteWaterValuesAsync(int waterValuesId);
    }
}