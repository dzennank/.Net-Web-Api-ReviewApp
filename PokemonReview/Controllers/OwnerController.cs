using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Interfaces;
using PokemonReview.Models;
using PokemonReview.Repository;

namespace PokemonReview.Controllers
{
    [Route("api/owner")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Owner>))]

        public IActionResult GetOwners()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners()).ToList();
            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]

        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));
            return Ok(owner);
        }

        [HttpGet("owners/{countryId}")]
        [ProducesResponseType(200, Type = typeof(List<Owner>))]
        [ProducesResponseType(400)]

        public IActionResult GetOwnersByCountry(int countryId)
        {
           
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnersFromACountry(countryId));
            return Ok(owners);
        }

        [HttpPost("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromBody] OwnerDto owner, int countryId)
        {
            if (owner == null)
                return BadRequest(ModelState);
            var ownerName = _ownerRepository.GetOwners().Where(o => o.LastName.ToUpper() == owner.LastName.ToUpper()).FirstOrDefault();
            if (ownerName != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var ownerMapp = _mapper.Map<Owner>(owner);
            
            ownerMapp.Country = _countryRepository.GetCountry(countryId);
            if (!_ownerRepository.CreateOwner(ownerMapp))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Sucessfully created");
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateOwner([FromBody] OwnerDto ownerUpdate, int ownerId)
        {
            if (ownerUpdate == null)
                return BadRequest(ModelState);

            if (ownerId != ownerUpdate.Id)
                return BadRequest(ModelState);

            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var owner = _mapper.Map<Owner>(ownerUpdate);
            if (!_ownerRepository.UpdateOwner(owner))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Sucessfully updated");

        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var owner = _ownerRepository.GetOwner(ownerId);

            if (!_ownerRepository.DeleteOwner(owner))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Sucessfully deleted");

        }
    }
}
