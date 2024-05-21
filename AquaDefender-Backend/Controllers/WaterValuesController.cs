using Microsoft.AspNetCore.Mvc;
using AquaDefender_Backend.Services.Interfaces;
using AquaDefender_Backend.DTOs;
using AquaDefender_Backend.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Service.Interfaces;

namespace AquaDefender_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WaterValuesController : ControllerBase
    {
        private readonly IWaterValuesService _waterValuesService;

        public WaterValuesController(IWaterValuesService waterValuesService)
        {
            _waterValuesService = waterValuesService;
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

            var waterValues = await _waterValuesService.GetWaterValuesByIdAsync(id);

            if (waterValues == null)
            {
                return NotFound($"Valorile de apă cu ID-ul {id} nu au fost găsite.");
            }

            return Ok(waterValues);
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

            var waterValues = await _waterValuesService.GetAllWaterValuesByWaterInfoIdAsync(waterInfoId);

            if (waterValues == null)
            {
                return NotFound($"Nu au fost găsite valori de apă pentru informația despre apă cu ID-ul {waterInfoId}.");
            }

            return Ok(waterValues);
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
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
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

            var existingWaterValues = await _waterValuesService.GetWaterValuesByIdAsync(id);

            if (existingWaterValues == null)
            {
                return NotFound($"Valorile de apă cu ID-ul {id} nu au fost găsite.");
            }

            try
            {
                existingWaterValues.Name = waterValuesDto.Name;
                existingWaterValues.MaximumAllowedValue = waterValuesDto.MaximumAllowedValue;
                existingWaterValues.UserProvidedValue = waterValuesDto.UserProvidedValue;
                existingWaterValues.MeasurementUnit = waterValuesDto.MeasurementUnit;

                await _waterValuesService.UpdateWaterValuesAsync(existingWaterValues);

                return Ok(waterValuesDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

            var waterValues = await _waterValuesService.GetWaterValuesByIdAsync(id);

            if (waterValues == null)
            {
                return NotFound($"Valorile de apă cu ID-ul {id} nu au fost găsite.");
            }

            try
            {
                await _waterValuesService.DeleteWaterValuesAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
