using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LoginAndRegister.Models
{
    public class ApplicationUsers : IdentityUser
    {
        [MaxLength(100)]
        public string FristName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }

        public byte[]? ProfilePic { get; set; }
    }
}
