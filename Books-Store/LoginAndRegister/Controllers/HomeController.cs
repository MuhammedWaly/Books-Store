using LoginAndRegister.Models;
using LoginAndRegister.Reposaitory;
using LoginAndRegister.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LoginAndRegister.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeReposaitory _context;
        private readonly SignInManager<ApplicationUsers> _signInManager;

        public HomeController(ILogger<HomeController> logger, IHomeReposaitory context, SignInManager<ApplicationUsers> signInManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string strem = "", int GenreId = 0)
        {
            var Books = await _context.GetBooks(strem, GenreId);
            IEnumerable<Genre> genres = await _context.GetGenres();
            var bookModel = new BookDisplayModel()
            { 
                Books=Books,
                Genres=genres,
                strem=strem,
                GenreId=GenreId
            };

            return View(bookModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
    }
}