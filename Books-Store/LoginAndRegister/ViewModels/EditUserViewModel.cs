using System.ComponentModel.DataAnnotations;

namespace LoginAndRegister.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set;}
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Username")]
        public string Username { get; set; }

      
    }
}
