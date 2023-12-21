using LoginAndRegister.Models;
using System.ComponentModel.DataAnnotations;

namespace LoginAndRegister.ViewModels
{
    public class ProductForm
    {
        public int? Id { get; set; }

        [ StringLength(40)]
        public string BookName { get; set; }
        [Display(Name ="")]
        public IFormFile? Image { get; set; }
        public byte[]? ImageBytes { get; set; }
        [MaxLength(40)]
        public string AuthorName { get; set; }

        public IEnumerable<Genre>? Genres { get; set; }
        
        public double Price { get; set; }
        [Display(Name = "Genre")]
        public int GenresId { get; set; }
    }
}
