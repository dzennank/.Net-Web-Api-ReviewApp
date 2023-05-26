using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Interfaces;
using PokemonReview.Models;
using PokemonReview.Repository;
using System.Diagnostics.Metrics;

namespace PokemonReview.Controllers
{
    [Route("api/country")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Country>))]

        public IActionResult GetCountries()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
            return Ok(countries);

        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]

        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));
                return Ok(country);
        }

        [HttpGet("country/{ownerId}")]
        [ProducesResponseType(200, Type =typeof(Country))]
        [ProducesResponseType(400)]

        public IActionResult GetCountryByOwner(int ownerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));
            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateCountry([FromBody] CountryDto country)
        {
            if (country == null)
                return BadRequest(ModelState);
            var countryName = _countryRepository.GetCountries().Where(c => c.Name.ToUpper() == country.Name.ToUpper()).FirstOrDefault();
            if(countryName != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var countryMap = _mapper.Map<Country>(country);

            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Sucessfully created");
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateCountry([FromBody] CountryDto countryUpdate, int countryId)
        {
            if (countryUpdate == null)
                return BadRequest(ModelState);

            if (countryId != countryUpdate.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var country = _mapper.Map<Country>(countryUpdate);
            if (!_countryRepository.UpdateCountry(country))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Sucessfully updated");

        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var country = _countryRepository.GetCountry(countryId);

            if (!_countryRepository.DeleteCountry(country))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Sucessfully deleted");

        }

    }
}
