using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();

        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);

        decimal GetPokemonRating(int pokeId);

        bool createPokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool Save();
        bool PokemonExists(int pokeId);
    }
}
