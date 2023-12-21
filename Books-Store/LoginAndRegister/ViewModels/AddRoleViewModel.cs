using System.ComponentModel.DataAnnotations;

namespace LoginAndRegister.ViewModels
{
    public class AddRoleViewModel
    {
        [MaxLength(50)]
        [Required(ErrorMessage ="Please Enter A Role")]
        
        public string Name { get; set; }
    }
}
