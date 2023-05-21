using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface IReviewRepository
    {
         Review GetReview(int id);
        List<Review> GetReviews();
        Review GetReviewByReviewer(int reviewerId);
        List<Review> GetReviewByPokemon(int pokeId);

        bool ReviewExists(int reviewId);

    }
}
