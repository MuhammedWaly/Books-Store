using LoginAndRegister.Models;

namespace LoginAndRegister.ViewModels
{
    public class BookDisplayModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public string strem { get; set; } = string.Empty;
        public int GenreId { get; set; } = 0;

    }
}
