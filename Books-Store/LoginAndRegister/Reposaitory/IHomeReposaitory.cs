using LoginAndRegister.Models;

namespace LoginAndRegister.Reposaitory
{
    public interface IHomeReposaitory
    {
       Task<IEnumerable<Book>> GetBooks(string searchTerm = "", int genreId = 0);

        Task<IEnumerable<Genre>> GetGenres();
    }
}