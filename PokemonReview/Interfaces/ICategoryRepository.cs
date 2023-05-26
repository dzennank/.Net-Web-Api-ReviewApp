using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonByCategory(int categoryId);

        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool Save();

        bool CategoriesExists(int id);
    }
}   
