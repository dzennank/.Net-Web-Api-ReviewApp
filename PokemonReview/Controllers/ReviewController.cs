using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
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
    }
}
