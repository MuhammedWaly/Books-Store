using LoginAndRegister.Data;
using LoginAndRegister.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginAndRegister.Reposaitory
{
    public class HomeReposaitory : IHomeReposaitory
    {
        private readonly ApplicationDbContext _context;

        public HomeReposaitory(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Book>> GetBooks(string searchTerm = "", int genreId = 0)
        {
            searchTerm = searchTerm.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                var query = from book in _context.Books
                            join genre in _context.Genres on book.GenreId equals genre.Id
                            select new Book
                            {
                                BookName = book.BookName,
                                AuthorName = book.AuthorName,
                                GenreId = genre.Id,
                                Id = book.Id,
                                Price = book.Price,
                                Image = book.Image,
                                GenreName = genre.GenreName
                            };

                if (genreId > 0)
                {
                    query = query.Where(a => a.GenreId == genreId);
                }

                return await query.ToListAsync();
            }

            // If searchTerm is not empty, apply the filter and then execute the query.
            return await (from book in _context.Books
                          join genre in _context.Genres on book.GenreId equals genre.Id
                          where book != null && book.BookName.ToLower().Contains(searchTerm.ToLower())
                          select new Book
                          {
                              BookName = book.BookName,
                              AuthorName = book.AuthorName,
                              GenreId = genre.Id,
                              Id = book.Id,
                              Price = book.Price,
                              Image = book.Image,
                              GenreName = genre.GenreName
                          }).ToListAsync();
        }



        public async Task<IEnumerable<Genre>> GetGenres()
        {
            return (await _context.Genres.ToListAsync());
        }
    }
}
