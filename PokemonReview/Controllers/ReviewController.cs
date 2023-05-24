using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Interfaces;
using PokemonReview.Models;
using PokemonReview.Repository;

namespace PokemonReview.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviwerRepository reviwerRepository;
        private readonly IPokemonRepository pokemonRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IReviwerRepository reviwerRepository, IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            this.reviwerRepository = reviwerRepository;
            this.pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]

        public IActionResult GetReview(int reviewId)
        {
                if(!_reviewRepository.ReviewExists(reviewId))
                    return NotFound();
                if(!ModelState.IsValid) 
                    return BadRequest(ModelState);
                var getReview = _reviewRepository.GetReview(reviewId);
                var review = _mapper.Map<ReviewDto>(getReview);
                return Ok(review);   
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Review>))]

        public IActionResult GetReviews()
        {
                if(!ModelState.IsValid)
                   return BadRequest(ModelState);
                var getReviews = _reviewRepository.GetReviews();
                var reviews = _mapper.Map<List<ReviewDto>>(getReviews).ToList();
                return Ok(reviews);
                
        }
        [HttpGet("reviewByReviewer/{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Review))]

        public IActionResult GetReviewByReviewers(int reviewerId)
        {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                var getReview = _reviewRepository.GetReviewByReviewer(reviewerId);
                var review = _mapper.Map<ReviewDto>(getReview);
                return Ok(review);
        }

        [HttpGet("reviewsForPokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(List<Review>))]

        public IActionResult GetReviewByPokemon(int pokeId)
        {
                if(!ModelState.IsValid) 
                    return BadRequest(ModelState);
                var getReviews = _reviewRepository.GetReviewByPokemon(pokeId);
                var reviews = _mapper.Map<List<ReviewDto>>(getReviews).ToList();
                return Ok(reviews);
        }
        [HttpPost("{ReviewerId}/{PokemonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromBody] ReviewDto review, int ReviewerId, int PokemonId)
        {
            if (review == null)
                return BadRequest(ModelState);
            var reviewTitle = _reviewRepository.GetReviews().Where(o => o.Title.ToUpper() == review.Title.ToUpper()).FirstOrDefault();
            if (reviewTitle != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewMapp = _mapper.Map<Review>(review);

            reviewMapp.Reviewer = reviwerRepository.GetReviewer(ReviewerId);
            reviewMapp.Pokemon = pokemonRepository.GetPokemon(PokemonId);
            if (!_reviewRepository.CreateReview(reviewMapp))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Sucessfully created");
        }
    }
}
