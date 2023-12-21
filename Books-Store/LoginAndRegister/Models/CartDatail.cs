using System.ComponentModel.DataAnnotations;

namespace LoginAndRegister.Models
{
    public class CartDatail
    {
        public int Id { get; set; }
        [Required]

        public int ShoppingcartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
        [Required]
        public double UnitPrice {  get; set; }

        [Required]
        public int BookId { get; set; }

        public Book Book { get; set; }

        public int Quantity { get; set; }   
    }
}
