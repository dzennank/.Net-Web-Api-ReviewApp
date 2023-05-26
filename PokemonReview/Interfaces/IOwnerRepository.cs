using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface IOwnerRepository
    {
        List<Owner> GetOwners();
        Owner GetOwner(int id);
        List<Owner> GetOwnersFromACountry(int countryId);
        bool CreateOwner(Owner owner);

        bool UpdateOwner(Owner owner);

        bool Save();
        bool OwnerExists(int id);

    }
}
