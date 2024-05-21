using AquaDefender_Backend.Domain;
using AquaDefender_Backend.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaDefender_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
        }

        [HttpGet("cities")]
        public async Task<ActionResult<IEnumerable<City>>> GetAllCities()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cities = await _locationService.GetAllCitiesAsync();
            return Ok(cities);
        }

        [HttpGet("counties")]
        public async Task<ActionResult<IEnumerable<County>>> GetAllCounties()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var counties = await _locationService.GetAllCountiesAsync();
            return Ok(counties);
        }

        [HttpGet("cities/id/{id}")]
        public async Task<ActionResult<City>> GetCityById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = await _locationService.GetCityByIdAsync(id);
            if (city == null)
            {
                return NotFound($"Orașul cu ID-ul {id} nu a fost găsit.");
            }
            return Ok(city);
        }

        [HttpGet("counties/id/{id}")]
        public async Task<ActionResult<County>> GetCountyById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var county = await _locationService.GetCountyByIdAsync(id);
            if (county == null)
            {
                return NotFound($"Județul cu ID-ul {id} nu a fost găsit.");
            }
            return Ok(county);
        }

        [HttpGet("counties/{countyId}/cities")]
        public async Task<ActionResult<List<City>>> GetAllCitiesByCountyId(int countyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cities = await _locationService.GetAllCitiesByCountyIdAsync(countyId);
            if (cities == null || cities.Count == 0)
            {
                return NotFound($"Orașele pentru județul cu ID-ul {countyId} nu au fost găsite.");
            }
            return Ok(cities);
        }

        [HttpGet("cities/{cityName}")]
        public async Task<ActionResult<City>> GetCityByName(string cityName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = await _locationService.GetCityByNameAsync(cityName);
            if (city == null)
            {
                return NotFound($"Orașul cu numele {cityName} nu a fost găsit.");
            }
            return Ok(city);
        }

        [HttpGet("counties/{countyName}")]
        public async Task<ActionResult<County>> GetCountyByName(string countyName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var county = await _locationService.GetCountyByNameAsync(countyName);
            if (county == null)
            {
                return NotFound($"Județul cu numele {countyName} nu a fost găsit.");
            }
            return Ok(county);
        }

    }
}
