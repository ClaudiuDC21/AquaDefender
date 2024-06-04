using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaDefender_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(ILocationService locationService, ILogger<LocationsController> logger)
        {
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("cities")]
        public async Task<ActionResult<IEnumerable<City>>> GetAllCities()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var cities = await _locationService.GetAllCitiesAsync();
                return Ok(cities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all cities.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("counties")]
        public async Task<ActionResult<IEnumerable<County>>> GetAllCounties()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var counties = await _locationService.GetAllCountiesAsync();
                return Ok(counties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all counties.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("cities/id/{id}")]
        public async Task<ActionResult<City>> GetCityById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var city = await _locationService.GetCityByIdAsync(id);
                if (city == null)
                {
                    return NotFound($"Orașul cu ID-ul {id} nu a fost găsit.");
                }
                return Ok(city);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the city with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("counties/id/{id}")]
        public async Task<ActionResult<County>> GetCountyById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var county = await _locationService.GetCountyByIdAsync(id);
                if (county == null)
                {
                    return NotFound($"Județul cu ID-ul {id} nu a fost găsit.");
                }
                return Ok(county);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the county with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("counties/{countyId}/cities")]
        public async Task<ActionResult<List<City>>> GetAllCitiesByCountyId(int countyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var cities = await _locationService.GetAllCitiesByCountyIdAsync(countyId);
                if (cities == null || cities.Count == 0)
                {
                    return NotFound($"Orașele pentru județul cu ID-ul {countyId} nu au fost găsite.");
                }
                return Ok(cities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting cities for the county with ID {countyId}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("cities/{cityName}")]
        public async Task<ActionResult<City>> GetCityByName(string cityName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var city = await _locationService.GetCityByNameAsync(cityName);
                if (city == null)
                {
                    return NotFound($"Orașul cu numele {cityName} nu a fost găsit.");
                }
                return Ok(city);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the city with name {cityName}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("counties/{countyName}")]
        public async Task<ActionResult<County>> GetCountyByName(string countyName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var county = await _locationService.GetCountyByNameAsync(countyName);
                if (county == null)
                {
                    return NotFound($"Județul cu numele {countyName} nu a fost găsit.");
                }
                return Ok(county);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the county with name {countyName}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("city/{cityId}/email")]
        public async Task<IActionResult> GetCityEmail(int cityId)
        {
            if (cityId <= 0)
            {
                return BadRequest("City ID must be a positive integer.");
            }

            try
            {
                var email = await _locationService.GetCityEmailByIdAsync(cityId);
                if (email == null)
                {
                    return NotFound($"City with ID {cityId} not found.");
                }
                return Ok(new { email = email });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"Validation error occurred while getting the email for city with ID {cityId}.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the email for city with ID {cityId}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("county/{countyId}/email")]
        public async Task<IActionResult> GetCountyEmail(int countyId)
        {
            if (countyId <= 0)
            {
                return BadRequest("County ID must be a positive integer.");
            }

            try
            {
                var email = await _locationService.GetCountyEmailByIdAsync(countyId);
                if (email == null)
                {
                    return NotFound($"County with ID {countyId} not found.");
                }
                return Ok(new { email = email });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"Validation error occurred while getting the email for county with ID {countyId}.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the email for county with ID {countyId}.");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
