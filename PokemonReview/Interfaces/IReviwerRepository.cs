using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface IReviwerRepository
    {
        List<Reviewer> GetReviewers();
        Reviewer GetReviewer(int id);
        List<Review> GetReviewByReviewer(int reviewerId);

        bool ReviewerExists(int id);
    }
}
