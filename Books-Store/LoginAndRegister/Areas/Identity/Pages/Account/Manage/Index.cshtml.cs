// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Humanizer;
using LoginAndRegister.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoginAndRegister.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly SignInManager<ApplicationUsers> _signInManager;

        private List<string> _allowedExtensions = new List<string>() { ".jpg", ".png" };
        private long _maxAllowedSize = 1048576;

        public IndexModel(
            UserManager<ApplicationUsers> userManager,
            SignInManager<ApplicationUsers> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// 

            [MaxLength(15)]
            [Display(Name = "Frist Name")]
            public string FristName { get; set; }


            [MaxLength(15)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }



            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }


            [Display(Name = "Profile Picture")]
            public byte[] ProfilePic { get; set; }
        }

        private async Task LoadAsync(ApplicationUsers user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);


            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FristName = user.FristName,
                LastName = user.LastName,
                ProfilePic = user.ProfilePic

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            //gpte help there is exciption here


            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {


            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            var fristName = user.FristName;
            var LastName = user.LastName;
            if (Input.FristName != fristName)
            {
                user.FristName = Input.FristName;
                await _userManager.UpdateAsync(user);

            }


            if (Input.LastName != LastName)
            {
                user.LastName = Input.LastName;
                await _userManager.UpdateAsync(user);

            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (Request.Form.Count > 0)
            {

                var file = Request.Form.Files.FirstOrDefault();

                if (user.ProfilePic != null)
                {
                    if (!_allowedExtensions.Contains(Path.GetExtension(file.FileName.ToLower())))
                        return BadRequest("Invalid Extension, Only .png and .jpg");

                    if (file.FileName.Length > _maxAllowedSize)
                        return BadRequest("File must Be 1MB or smaller");    
                }
                using var dataStrem = new MemoryStream();
                await file.CopyToAsync(dataStrem);
                user.ProfilePic = dataStrem.ToArray();
                await _userManager.UpdateAsync(user);

            }



            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
