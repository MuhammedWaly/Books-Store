using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginAndRegister.Models
{
    public class Book
    {
        public  int Id { get; set; }
        [Required,MaxLength(40)]
        public string? BookName { get; set; }
        
        public byte[] Image { get; set; }
        public string? AuthorName { get; set; }
        public int GenreId { get; set; }
        public Genre genre { get; set; }
        public double Price {  get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<CartDatail> CartDetails { get; set; }
        [NotMapped]
        public string GenreName {  get; set; }

    }
}
