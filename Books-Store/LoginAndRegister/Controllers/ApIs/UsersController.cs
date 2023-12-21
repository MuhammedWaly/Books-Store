using LoginAndRegister.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoginAndRegister.Controllers.ApIs
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUsers> _userManger;
        

        public UsersController(UserManager<ApplicationUsers> userManger)
        {
            _userManger = userManger;
        }



        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManger.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result= await _userManger.DeleteAsync(user);
            if(!result.Succeeded) { throw new Exception(); }

            return Ok();
        }
    }
}
