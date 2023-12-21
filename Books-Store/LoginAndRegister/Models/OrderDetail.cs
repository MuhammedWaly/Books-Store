using System.ComponentModel.DataAnnotations;

namespace LoginAndRegister.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Unitprice { get; set; }

        public Order Order { get; set; }
        public Book book { get; set; }
        

    }
}
