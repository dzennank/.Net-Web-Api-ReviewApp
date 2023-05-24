using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Interfaces;
using PokemonReview.Models;
using PokemonReview.Repository;
using System.Collections.Generic;

namespace PokemonReview.Controllers
{
    [Route("api/pokemon")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Pokemon>))]
        public IActionResult GetPokemons()
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons()));
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            return Ok(_mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId)));
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]

        public IActionResult GetRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_pokemonRepository.GetPokemonRating(pokeId));
        }

        [HttpPost("{ownerId}/{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreatePokemon([FromBody] PokemonDto pokemon, int ownerId, int categoryId)
        {
            if(pokemon == null) 
                 return BadRequest(ModelState);
            var pokemonName = _pokemonRepository.GetPokemons().Where(o => o.Name.ToUpper() == pokemon.Name.ToUpper()).FirstOrDefault();
            if (pokemonName != null)
            {
                ModelState.AddModelError("", "pokemon already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var pokemonMap = _mapper.Map<Pokemon>(pokemon);
            
            if(!_pokemonRepository.createPokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Pokemon sucesfully created");

        }
    }
}
