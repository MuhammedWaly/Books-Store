using LoginAndRegister.Data;
using LoginAndRegister.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Claims;

namespace LoginAndRegister.Reposaitory
{
    public class UserOrderRepository: IUserOrderRepository
    {
        public readonly ApplicationDbContext _context;
        public readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUsers> _userManger;

        public UserOrderRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor, UserManager<ApplicationUsers> userManger)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userManger = userManger;
        }

        public async Task<IEnumerable<Order>> UserOrder()
        {
            var UserId = GetUserId();
            if (string.IsNullOrEmpty(UserId)) 
            {
                throw new Exception("User is not regestired");
            
            }
            var Orders = await _context.Orders.Include(a => a.OrderStatus)
                .Include(a => a.OrderDetail)
                .ThenInclude(x => x.book)
                .ThenInclude(a => a.genre)
                .Where(x => x.UserId == UserId).ToListAsync();
            return Orders;
        }


        private string GetUserId()
        {
            ClaimsPrincipal user = _contextAccessor.HttpContext.User;
            var userId = _userManger.GetUserId(user);
            return userId;
        }
    }
}
