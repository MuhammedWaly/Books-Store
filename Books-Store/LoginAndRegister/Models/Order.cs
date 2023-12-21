using System.ComponentModel.DataAnnotations;

namespace LoginAndRegister.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }= DateTime.UtcNow;
        [Required]
        public int OrderStatusId {  get; set; }
        public OrderStatus OrderStatus {  get; set; }
        public bool IsDeleted { get; set; }= false;
        public List<OrderDetail> OrderDetail {  get; set; }

    }
}
