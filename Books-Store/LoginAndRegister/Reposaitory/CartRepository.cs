using LoginAndRegister.Data;
using LoginAndRegister.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace LoginAndRegister.Reposaitory
{
    public class CartRepository: ICartRepository
    {
        public readonly ApplicationDbContext _context;
        public readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUsers> _userManger;


        public CartRepository(UserManager<ApplicationUsers> userManger, ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _userManger = userManger;
            _context = context;
            _contextAccessor = contextAccessor;
        }


        public async Task<int> AddItem(int BookId, int Qty)
        {

            string userId = GetUserId();

            using var Transaction = _context.Database.BeginTransaction();
            try
            {
                if (userId == null)
                {
                    throw new Exception("Invalid user Id"); // Indicate failure with a specific value
                }

                var cart = await GetCart(userId);

                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId,
                    };
                    _context.ShoppingCarts.Add(cart);
                    _context.SaveChanges();
                }

                var cardItem = _context.CartDatails.FirstOrDefault(a => a.ShoppingcartId == cart.Id && a.BookId == BookId);
                if (cardItem is not null)
                {
                    cardItem.Quantity += Qty;
                }
                else
                {
                    var book = _context.Books.Find(BookId);
                    cardItem = new CartDatail
                    {
                        BookId = BookId,
                        ShoppingcartId = cart.Id,
                        Quantity = Qty,
                        UnitPrice = book.Price
                    };
                    _context.CartDatails.Add(cardItem);
                }

                _context.SaveChanges();
                Transaction.Commit();
                //return 1; // Indicate success with a specific value
            }
            catch (Exception ex)
            {
               
            }
            var CardItemsCount = await GetCartItems(userId);
            return CardItemsCount;
        }


        public async Task<int> GetCartItems(string userId="")
        {
            if(!string.IsNullOrEmpty(userId)) 
            {
                userId = GetUserId();
            }
            var data = await (from cart in _context.ShoppingCarts
                              join CartDatail in _context.CartDatails
                              on cart.Id equals CartDatail.ShoppingcartId
                              select new { CartDatail.Id }).ToListAsync();
            return data.Count;
        }


        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId is null)
                throw new Exception("Invalid User ID");
            var shoppingCart = await _context.ShoppingCarts
                                       .Include(a => a.CartDetails)
                                       .ThenInclude(a => a.Book)
                                       .ThenInclude(a => a.genre).FirstOrDefaultAsync();
            return shoppingCart;
        }



        public async Task<int> RemoveItem(int BookId)
        {
            string userId = GetUserId();

            try
            {
                if (userId == null)
                {
                    throw new Exception("Invalid user Id");
                }

                var cart = await GetCart(userId);

                if (cart == null)
                {
                    throw new Exception("No cart created");
                }

                var cardItem = _context.CartDatails.FirstOrDefault(a => a.ShoppingcartId == cart.Id && a.BookId == BookId);

                if (cardItem == null)
                {
                    throw new Exception("Item not found in the cart");
                }
                else if (cardItem.Quantity == 1)
                {
                    _context.CartDatails.Remove(cardItem);
                }
                else
                {
                    cardItem.Quantity = cardItem.Quantity - 1;
                    _context.CartDatails.Update(cardItem); // Use Update instead of Add
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately (e.g., logging)
            }

            var CardItemsCount = await GetCartItems(userId);
            return CardItemsCount;
        }



        public async Task<ShoppingCart> GetCart(string userId)
        {

            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }


        private string GetUserId()
        {
            ClaimsPrincipal user = _contextAccessor.HttpContext.User;
            var userId = _userManger.GetUserId(user);
            return userId;
        }


        public async Task<int> RemoveBookFromCart(int BookId)
        {
            string userId = GetUserId();

            try
            {
                if (userId == null)
                {
                    throw new Exception("Invalid user Id");
                }

                var cart = await GetCart(userId);

                if (cart == null)
                {
                    throw new Exception("No cart created");
                }

                var cartItems = _context.CartDatails.Where(a => a.ShoppingcartId == cart.Id && a.BookId == BookId).ToList();

                if (cartItems == null || cartItems.Count == 0)
                {
                    throw new Exception("Book not found in the cart");
                }

                foreach (var cartItem in cartItems)
                {
                    _context.CartDatails.Remove(cartItem);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
               
            }

            var CardItemsCount = await GetCartItems(userId);
            return CardItemsCount;
        }

        public async Task<bool> Docheck()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception();

                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid Cart");

                var cartdetail = _context.CartDatails
                                 .Where(a => a.ShoppingcartId == cart.Id).ToList();
                if (cartdetail.Count == 0)
                    throw new Exception("cart is empty");

                var order = new Order
                {
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow,
                    OrderStatusId = 1
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
                foreach (var item in cartdetail)
                {
                    var orderDetail = new OrderDetail
                    { 
                        BookId=item.BookId,
                        OrderId=order.Id,
                        Quantity= item.Quantity,
                        Unitprice = item.UnitPrice
                    
                    };
                    _context.OrderDetails.Add(orderDetail);
                }
                _context.SaveChanges();

                // removing after checkout
                _context.CartDatails.RemoveRange(cartdetail);
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

    }
}
