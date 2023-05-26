using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Interfaces;
using PokemonReview.Models;
using PokemonReview.Repository;

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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromBody] ReviewerDto reviewer)
        {
            if (reviewer == null)
                return BadRequest(ModelState);
            var reviewerName = _reviwerRepository.GetReviewers().Where(o => o.LastName.ToUpper() == reviewer.LastName.ToUpper()).FirstOrDefault();
            if (reviewerName != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewerMapp = _mapper.Map<Reviewer>(reviewer);

            if (!_reviwerRepository.CreateReviewer(reviewerMapp))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Sucessfully created");

        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateCategory([FromBody] ReviewerDto reviewerUpdate, int reviewerId)
        {
            if (reviewerUpdate == null)
                return BadRequest(ModelState);

            if (reviewerId != reviewerUpdate.Id)
                return BadRequest(ModelState);

            if (!_reviwerRepository.ReviewerExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewer = _mapper.Map<Reviewer>(reviewerUpdate);
            if (!_reviwerRepository.UpdateReviewer(reviewer))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Sucessfully updated");

        }
    }
}
