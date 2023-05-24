using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface IReviwerRepository
    {
        List<Reviewer> GetReviewers();
        Reviewer GetReviewer(int id);
        List<Review> GetReviewByReviewer(int reviewerId);

        bool CreateReviewer(Reviewer reviewer);

        bool Save();

        bool ReviewerExists(int id);
    }
}
