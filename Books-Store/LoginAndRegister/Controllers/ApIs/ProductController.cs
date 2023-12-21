using LoginAndRegister.Data;
using LoginAndRegister.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginAndRegister.Controllers.ApIs
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }



        [HttpDelete]
        public async Task<IActionResult> DeleteBook(int BookId)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b=>b.Id==BookId);
            if (book == null) return NotFound();

            var result = _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
