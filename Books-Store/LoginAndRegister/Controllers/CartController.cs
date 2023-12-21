using LoginAndRegister.Data;
using LoginAndRegister.Models;
using LoginAndRegister.Reposaitory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace LoginAndRegister.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IToastNotification _toastNotification;

        private readonly SignInManager<ApplicationUsers> _signInManager;

        public CartController(ICartRepository cartRepository, UserManager<ApplicationUsers> userManger, SignInManager<ApplicationUsers> signInManager, IToastNotification toastNotification)
        {
            _cartRepository = cartRepository;
            _signInManager = signInManager;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> AddItem(int bookId,int qty=1, int redirect=0)
        {
            
            var CartCount = await _cartRepository.AddItem(bookId,qty);
            if (redirect == 0)
                return Ok(CartCount);

            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> RemoveItem(int bookId)
        {    
            var book = await _cartRepository.RemoveItem(bookId);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }
        public async Task<IActionResult> GetTotalItemsInCart()
        {
            var cartItems = await _cartRepository.GetCartItems();
            return Ok(cartItems);
        }

        public async Task<IActionResult> Remove(int BookId)
        {
            var book = await _cartRepository.RemoveBookFromCart(BookId);
            return RedirectToAction("GetUserCart");
        } 
        
        public async Task<IActionResult> Checkout(int BookId)
        {
            var ischeckedOut = await _cartRepository.Docheck();
            if (!ischeckedOut)
            {
                ModelState.AddModelError("Order", "Something went wrong, try again later");
            }
            _toastNotification.AddSuccessToastMessage("Done , Go check \"My Orders\" Section");
            return RedirectToAction("Index","home");
        }
    }
}
