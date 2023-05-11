namespace PokemonReview.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        ICollection<PokemonCategory> PokemonCategories { get; set;}
    }
}
