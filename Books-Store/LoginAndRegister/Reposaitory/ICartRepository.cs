using LoginAndRegister.Models;

namespace LoginAndRegister.Reposaitory
{
    public interface ICartRepository
    {
        Task<int> AddItem(int BookId, int Qty);
        Task<ShoppingCart> GetUserCart();
        Task<int> RemoveItem(int BookId);
        Task<ShoppingCart> GetCart(string userId);
        Task<int> RemoveBookFromCart(int BookId);
        Task<int> GetCartItems(string userId = "");
        Task<bool> Docheck();
    }
}