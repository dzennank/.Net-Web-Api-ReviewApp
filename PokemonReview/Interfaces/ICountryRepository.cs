using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface ICountryRepository
    {
        List<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountryByOwner(int ownerId);
        List<Owner> GetOwnersFromACountry(int countryId);
        bool CountryExists(int id);
    }
}
