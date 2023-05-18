using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface IOwnerRepository
    {
        List<Owner> GetOwners();
        Owner GetOwner(int id);
        List<Owner> GetOwnersFromACountry(int countryId);
        bool OwnerExists(int id);

    }
}
