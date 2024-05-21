using Microsoft.AspNetCore.Mvc;
using AquaDefender_Backend.Services.Interfaces;
using AquaDefender_Backend.DTOs;
using AquaDefender_Backend.Domain;
using System.Threading.Tasks;
using System.Linq;

namespace AquaDefender_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WaterInfoController : ControllerBase
    {
        private readonly IWaterInfoService _waterInfoService;

        public WaterInfoController(IWaterInfoService waterInfoService)
        {
            _waterInfoService = waterInfoService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWaterInfoById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("Id-ul trebuie să fie un număr întreg pozitiv.");
            }

            var waterInfo = await _waterInfoService.GetWaterInfoByIdAsync(id);

            if (waterInfo == null)
            {
                return NotFound($"Informația despre apă cu ID-ul {id} nu a fost găsită.");
            }

            return Ok(waterInfo);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllWaterInfos()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var waterInfos = await _waterInfoService.GetAllWaterInfosAsync();
            return Ok(waterInfos);
        }

        [HttpGet("city/{cityId}")]
        public async Task<IActionResult> GetAllWaterInfosByCityId(int cityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (cityId <= 0)
            {
                return BadRequest(new { message = "Id-ul orașului trebuie să fie un număr întreg pozitiv." });
            }

            var waterInfos = await _waterInfoService.GetAllWaterInfosByCityIdAsync(cityId);

            if (waterInfos == null || waterInfos.Count == 0)
            {
                return NotFound(new { message = "Nu au fost găsite informații despre apă pentru orașul specificat." });
            }

            return Ok(waterInfos);
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReportByDateAndCity([FromQuery] DateTime date, [FromQuery] int cityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (cityId <= 0)
            {
                return BadRequest("Id-ul orașului trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                var report = await _waterInfoService.GetReportByDateAndCityAsync(date, cityId);
                if (report == null)
                {
                    return NotFound($"Nu a fost găsit niciun raport pentru data specificată {date.ToShortDateString()} și ID-ul orașului {cityId}.");
                }
                return Ok(report);
            }
            catch (Exception)
            {
                return StatusCode(500, "A apărut o eroare în timpul procesării cererii.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<WaterInfoDto>> CreateWaterInfo([FromBody] WaterInfoDto waterInfoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingReport = await _waterInfoService.GetReportByDateAndCityAsync(waterInfoDto.DateReported, int.Parse(waterInfoDto.City));
                if (existingReport != null)
                {
                    return BadRequest("Un raport pentru această dată, județ și oraș există deja.");
                }

                var waterInfo = new WaterInfo
                {
                    Name = waterInfoDto.Name,
                    CountyId = int.Parse(waterInfoDto.County),
                    CityId = int.Parse(waterInfoDto.City),
                    DateReported = DateTime.Now,
                    AdditionalNotes = waterInfoDto.AdditionalNotes
                };

                await _waterInfoService.CreateWaterInfoAsync(waterInfo);

                return Ok(waterInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWaterInfo(int id, [FromBody] WaterInfoDto waterInfoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("Id-ul trebuie să fie un număr întreg pozitiv.");
            }

            var existingWaterInfo = await _waterInfoService.GetWaterInfoByIdAsync(id);

            if (existingWaterInfo == null)
            {
                return NotFound($"Informația despre apă cu ID-ul {id} nu a fost găsită.");
            }

            try
            {
                existingWaterInfo.Name = waterInfoDto.Name;
                existingWaterInfo.CountyId = existingWaterInfo.CountyId;
                existingWaterInfo.CityId = existingWaterInfo.CityId;
                existingWaterInfo.DateReported = existingWaterInfo.DateReported;
                existingWaterInfo.AdditionalNotes = waterInfoDto.AdditionalNotes;

                await _waterInfoService.UpdateWaterInfoAsync(existingWaterInfo);

                return Ok(waterInfoDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWaterInfo(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("Id-ul trebuie să fie un număr întreg pozitiv.");
            }

            var waterInfo = await _waterInfoService.GetWaterInfoByIdAsync(id);

            if (waterInfo == null)
            {
                return NotFound($"Informația despre apă cu ID-ul {id} nu a fost găsită.");
            }

            try
            {
                await _waterInfoService.DeleteWaterInfoAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
