using LoginAndRegister.Data;
using LoginAndRegister.Models;
using LoginAndRegister.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NToastNotify;
using SQLitePCL;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoginAndRegister.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;

        private List<string> _allowedExtensions = new List<string>() { ".jpg", ".png" };
        private long _maxAllowedSize = 1048576;

        public ProductController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }


        public async Task<IActionResult> AddBook()
        {

            var viewModel = new ProductForm
            {
                Genres = await _context.Genres.OrderBy(m => m.GenreName).ToListAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook([FromForm] ProductForm model)
        {

            if (!ModelState.IsValid)
            {
                model.Genres = await _context.Genres.OrderBy(m => m.GenreName).ToListAsync();
                return View(model);
            }
            var files = Request.Form.Files;
            if (!files.Any())
            {
                model.Genres = await _context.Genres.OrderBy(m => m.GenreName).ToListAsync();
                ModelState.AddModelError("Image", "Please Add Image");
                return View(model);

            }
            var Image = files.FirstOrDefault();

            if (!_allowedExtensions.Contains(Path.GetExtension(Image.FileName).ToLower()))
            //gpte solve the issue related to "Image" that may be null

            {
                model.Genres = await _context.Genres.OrderBy(m => m.GenreName).ToListAsync();
                ModelState.AddModelError("Image", "Only jpg and png are allowed");
                return View(model);
            }
            if (Image.Length > _maxAllowedSize)
            {
                model.Genres = await _context.Genres.OrderBy(m => m.GenreName).ToListAsync();
                ModelState.AddModelError("Image", "Image cannot be more than 1 MB");
                return View(model);
            }

            using var datastream = new MemoryStream();
            await Image.CopyToAsync(datastream);
            var book = new Book
            {
                BookName = model.BookName,
                Price = model.Price,
                AuthorName = model.AuthorName,
                GenreId = model.GenresId,
                Image = datastream.ToArray()

            };
            _context.Books.Add(book);
            _context.SaveChanges();
            _toastNotification.AddSuccessToastMessage("Book Aded successfully");

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(int? BookId)
        {
            var book = await _context.Books.FindAsync(BookId);
            if (book == null)
                return NotFound();

            var viewModel = new ProductForm
            {
                Id = BookId,
                BookName = book.BookName,
                AuthorName = book.AuthorName,
                Price = book.Price,
                ImageBytes = book.Image,
                GenresId=book.GenreId,
                Genres=await _context.Genres.OrderBy(m => m.GenreName).ToListAsync()
            };
            return View("Edit",viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ProductForm Dto)
        {
            if (!ModelState.IsValid)
            {
                Dto.Genres = await _context.Genres.OrderBy(m => m.GenreName).ToListAsync();
                return View("Edit", Dto);
            }

            var SelectedBook = await _context.Books.FindAsync(Dto.Id);

            if (SelectedBook == null) return BadRequest("No Book Found");

            var files = Request.Form.Files;

            if (files.Any())

            {
                //if (Dto.Image != null)
                //{
                    using (var memoryStream = new MemoryStream())
                    {
                        await Dto.Image.CopyToAsync(memoryStream);
                        Dto.ImageBytes = memoryStream.ToArray();
                    }
                    SelectedBook.Image = Dto.ImageBytes;
                //}
                // Validate file extension and size if needed

                // Assign the byte[] to SelectedBook.Image
                
            }

            // Update other properties
            SelectedBook.BookName = Dto.BookName;
            SelectedBook.AuthorName = Dto.AuthorName;
            SelectedBook.Price = Dto.Price;
            SelectedBook.GenreId = Dto.GenresId;

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }



}
