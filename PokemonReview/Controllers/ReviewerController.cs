using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Controllers
{
    [Route("api/reviewer")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviwerRepository _reviwerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviwerRepository reviwerRepository, IMapper mapper)
        {
            _reviwerRepository = reviwerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Reviewer>))]
        public IActionResult GetReviewers() 
        { 
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var getReviewers = _reviwerRepository.GetReviewers();
            var reviewers = _mapper.Map<List<ReviewerDto>>(getReviewers).ToList();
            return Ok(reviewers);

        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviwerRepository.ReviewerExists(reviewerId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var getReviewer = _reviwerRepository.GetReviewer(reviewerId);
            var reviewer = _mapper.Map<ReviewerDto>(getReviewer);
            return Ok(reviewer);

        }
        [HttpGet("reviewByReviewer/{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(List<Reviewer>))]

        public IActionResult GetReviewByReviewer(int reviewerId)
        {
            if(!_reviwerRepository.ReviewerExists(reviewerId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
             var getReview = _reviwerRepository.GetReviewByReviewer(reviewerId);
            var review = _mapper.Map<List<ReviewDto>>(getReview);
            return Ok(review);

        }
    }
}
