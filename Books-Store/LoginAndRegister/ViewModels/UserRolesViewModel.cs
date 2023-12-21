using System.ComponentModel.DataAnnotations;

namespace LoginAndRegister.ViewModels
{
    public class UserRolesViewModel
    {
        public string USerId {  get; set; }
        public string UserName { get; set; }
        [Required]
        public List<RoleViewModel> Roles { get; set; }
    }
}
