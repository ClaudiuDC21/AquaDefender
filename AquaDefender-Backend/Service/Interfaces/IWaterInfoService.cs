using AquaDefender_Backend.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaDefender_Backend.Services.Interfaces
{
    public interface IWaterInfoService
    {
        Task<WaterInfo> GetWaterInfoByIdAsync(int waterInfoId);
        Task<List<WaterInfo>> GetAllWaterInfosAsync();
        Task<List<WaterInfo>> GetAllWaterInfosByCityIdAsync(int cityId);
         Task<WaterInfo> GetReportByDateAndCityAsync(DateTime date, int cityId); 
        Task CreateWaterInfoAsync(WaterInfo waterInfo);
        Task UpdateWaterInfoAsync(WaterInfo waterInfo);
        Task DeleteWaterInfoAsync(int waterInfoId);
    }
}
