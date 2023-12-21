using LoginAndRegister.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginAndRegister.Controllers.Roles
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _userRole;

        public RolesController(RoleManager<IdentityRole> userRole)
        {
            _userRole = userRole;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _userRole.Roles.ToListAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(AddRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", await _userRole.Roles.ToListAsync());
            }


            if (await _userRole.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name","Role is already Exists");
                return View("Index", await _userRole.Roles.ToListAsync());
            }



            await _userRole.CreateAsync(new IdentityRole(model.Name.Trim()));
            return RedirectToAction(nameof(Index));

        }
    }
}
