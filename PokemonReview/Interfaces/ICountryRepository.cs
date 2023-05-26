using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface ICountryRepository
    {
        List<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountryByOwner(int ownerId);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);

        bool DeleteCountry(Country conutry);

        bool Save();
        bool CountryExists(int id);
    }
}
