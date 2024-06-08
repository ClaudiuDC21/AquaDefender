using Microsoft.AspNetCore.Mvc;
using AquaDefender_Backend.DTOs;
using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Service.Interfaces;

namespace AquaDefender_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WaterValuesController : ControllerBase
    {
        private readonly IWaterValuesService _waterValuesService;
        private readonly ILogger<WaterValuesController> _logger;

        public WaterValuesController(IWaterValuesService waterValuesService, ILogger<WaterValuesController> logger)
        {
            _waterValuesService = waterValuesService ?? throw new ArgumentNullException(nameof(waterValuesService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWaterValuesById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("Id-ul trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                var waterValues = await _waterValuesService.GetWaterValuesByIdAsync(id);

                if (waterValues == null)
                {
                    return NotFound($"Valorile de apă cu ID-ul {id} nu au fost găsite.");
                }

                return Ok(waterValues);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the water values with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("waterinfo/{waterInfoId}")]
        public async Task<IActionResult> GetAllWaterValuesByWaterInfoId(int waterInfoId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (waterInfoId <= 0)
            {
                return BadRequest("Id-ul informației despre apă trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                var waterValues = await _waterValuesService.GetAllWaterValuesByWaterInfoIdAsync(waterInfoId);

                if (waterValues == null)
                {
                    return NotFound($"Nu au fost găsite valori de apă pentru informația despre apă cu ID-ul {waterInfoId}.");
                }

                return Ok(waterValues);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting water values for the water info with ID {waterInfoId}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllWaterValues()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var waterValues = await _waterValuesService.GetAllWaterValuesAsync();
                return Ok(waterValues);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all water values.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<WaterValuesDto>> CreateWaterValues([FromBody] WaterValuesDto waterValuesDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var waterValues = new WaterValues
                {
                    Name = waterValuesDto.Name,
                    MaximumAllowedValue = waterValuesDto.MaximumAllowedValue,
                    UserProvidedValue = waterValuesDto.UserProvidedValue,
                    MeasurementUnit = waterValuesDto.MeasurementUnit,
                    IdWaterInfo = waterValuesDto.IdWaterInfo
                };

                await _waterValuesService.CreateWaterValuesAsync(waterValues);

                var createdDto = new WaterValuesDto
                {
                    Id = waterValues.Id,
                    Name = waterValues.Name,
                    MaximumAllowedValue = waterValues.MaximumAllowedValue,
                    UserProvidedValue = waterValues.UserProvidedValue,
                    MeasurementUnit = waterValues.MeasurementUnit,
                    IdWaterInfo = waterValues.IdWaterInfo
                };

                return CreatedAtAction(nameof(GetWaterValuesById), new { id = waterValues.Id }, createdDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating new water values.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWaterValues(int id, [FromBody] WaterValuesDto waterValuesDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("Id-ul trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                var existingWaterValues = await _waterValuesService.GetWaterValuesByIdAsync(id);

                if (existingWaterValues == null)
                {
                    return NotFound($"Valorile de apă cu ID-ul {id} nu au fost găsite.");
                }

                existingWaterValues.Name = waterValuesDto.Name;
                existingWaterValues.MaximumAllowedValue = waterValuesDto.MaximumAllowedValue;
                existingWaterValues.UserProvidedValue = waterValuesDto.UserProvidedValue;
                existingWaterValues.MeasurementUnit = waterValuesDto.MeasurementUnit;

                await _waterValuesService.UpdateWaterValuesAsync(existingWaterValues);

                return Ok(waterValuesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the water values with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWaterValues(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                return BadRequest("Id-ul trebuie să fie un număr întreg pozitiv.");
            }

            try
            {
                var waterValues = await _waterValuesService.GetWaterValuesByIdAsync(id);

                if (waterValues == null)
                {
                    return NotFound($"Valorile de apă cu ID-ul {id} nu au fost găsite.");
                }

                await _waterValuesService.DeleteWaterValuesAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the water values with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
