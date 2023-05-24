﻿using PokemonReview.Data;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Repository
{
    public class ReviewerRepository : IReviwerRepository
    {
        private readonly DataContext _context;

        public ReviewerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return Save();
        }

        public List<Review> GetReviewByReviewer(int reviewerId)
        {
            return _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public Reviewer GetReviewer(int id)
        {
            return _context.Reviewers.Where(r => r.Id == id).FirstOrDefault();
        }

        public List<Reviewer> GetReviewers()
        {
            return _context.Reviewers.OrderBy(r => r.Id).ToList();
        }

        public bool ReviewerExists(int id)
        {
            return _context.Reviewers.Any(r => r.Id == id);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
