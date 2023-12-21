using LoginAndRegister.Models;
using LoginAndRegister.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoginAndRegister.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUsers> _userManger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUsers> userManger, RoleManager<IdentityRole> roleManager)
        {
            _userManger = userManger;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {

            var users = await _userManger.Users.Select(user => new UserViewMode
            {
                Id = user.Id,
                FristName = user.FristName,
                LasttName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Roles = _userManger.GetRolesAsync(user).Result
            }).ToListAsync();
            

            return View(users);
            
        }
        public async Task<IActionResult> EditRoles(string userId)
        {
            var user = await _userManger.FindByIdAsync(userId);
            if (user == null)
                return NotFound();
            var roles = await _roleManager.Roles.ToListAsync();
            var viewModel = new UserRolesViewModel
            {
                USerId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = _userManger.IsInRoleAsync(user, role.Name).Result

                }).ToList(),

            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(UserRolesViewModel Model)
        {
            var user = await _userManger.FindByIdAsync(Model.USerId);
            if (user == null)
                return NotFound();
            var userRoles = await _userManger.GetRolesAsync(user);
            if (!Model.Roles.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Roles", "Select at least one Role");
                return View(Model);
            }

            foreach (var roles in Model.Roles)
            {
                if (userRoles.Any(r => r == roles.RoleName) && !roles.IsSelected)
                {
                    await _userManger.RemoveFromRoleAsync(user, roles.RoleName);
                }
                if (!userRoles.Any(r => r == roles.RoleName) && roles.IsSelected)
                {
                    await _userManger.AddToRoleAsync(user, roles.RoleName);
                }

            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> AddNew()
        {
            var roles = await _roleManager.Roles.Select(r => new RoleViewModel { RoleId = r.Id, RoleName = r.Name,IsSelected=false}).ToListAsync();
            var viewModel = new AddUserViewModel
            {
                Roles = roles
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNew(AddUserViewModel NUSer)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return View(NUSer);
            }

            // Check if the user with the same email already exists
            if (await _userManger.FindByEmailAsync(NUSer.Email) != null)
            {
                ModelState.AddModelError("Email", "Email is already registered");
                return View(NUSer);
            }

            // Check if the user with the same username already exists
            if (await _userManger.FindByNameAsync(NUSer.Username) != null)
            {
                ModelState.AddModelError("Username", "Username is already registered");
                return View(NUSer);
            }

            // Check if at least one role is selected
            if (!NUSer.Roles.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Roles", "Select at least one Role");
                return View(NUSer);
            }

            var user = new ApplicationUsers
            {
                UserName = NUSer.Username,
                Email = NUSer.Email,
                FristName = NUSer.FirstName,
                LastName = NUSer.LastName,
            };

            // Attempt to create the user
            var result = await _userManger.CreateAsync(user, NUSer.Password);

            if (!result.Succeeded)
            {
                // If user creation fails, add errors to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(NUSer);
            }

            // Add user to selected roles
            await _userManger.AddToRolesAsync(user, NUSer.Roles.Where(r => r.IsSelected).Select(r => r.RoleName));

            // Clear ModelState for a clean state
            ModelState.Clear();

            // Redirect to Index upon successful user creation
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> EditUsers(string userId)
        {
            var user = await _userManger.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var viewModel = new EditUserViewModel
            {
                Id = userId,
                Email = user.Email,
                Username = user.UserName,
                FirstName = user.FristName,
                LastName = user.LastName

            };
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditUsers(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return NotFound();

            var user = await _userManger.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();

            var sameEmail = await _userManger.FindByEmailAsync(model.Email); 
            if(sameEmail!=null && sameEmail.Id!= model.Id)
            {
                ModelState.AddModelError("Email", "Email is already registered");
                return View(model);
            }

            var sameUsername = await _userManger.FindByNameAsync(model.Username);
            if (sameUsername!= null && sameUsername.Id != model.Id)
            {
                ModelState.AddModelError("Username", "Username is already registered");
                return View(model);
            }
     
            user.UserName = model.Username;
            user.FristName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            await _userManger.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        
    }

}
