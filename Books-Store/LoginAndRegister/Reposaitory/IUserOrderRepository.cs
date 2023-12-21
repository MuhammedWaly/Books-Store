using LoginAndRegister.Models;

namespace LoginAndRegister.Reposaitory
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrder();
    }
}