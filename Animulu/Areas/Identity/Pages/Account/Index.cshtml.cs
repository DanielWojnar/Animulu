using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Animulu.Data;
using Animulu.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
namespace Animulu.Areas.Identity.Pages.Account
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<AnimuluUser> _userManager;
        private readonly SignInManager<AnimuluUser> _signInManager;
        private readonly IWebHostEnvironment _hostEnv;

        public IndexModel(UserManager<AnimuluUser> userManager, SignInManager<AnimuluUser> signInManager, IWebHostEnvironment hostEnv)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostEnv = hostEnv;
        }
        public class MaxFileSizeAttribute : ValidationAttribute
        {
            private readonly int _maxFileSize;
            public MaxFileSizeAttribute(int maxFileSize)
            {
                _maxFileSize = maxFileSize * 1024 * 1024;
            }

            protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
            {
                var file = value as IFormFile;
                if (file != null)
                {
                    if (file.Length > _maxFileSize)
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }

                return ValidationResult.Success;
            }

            public string GetErrorMessage()
            {
                return $"Maximum allowed file size is { (_maxFileSize/1024/1024)} MB.";
            }
        }
        public class AllowedExtensionsAttribute : ValidationAttribute
        {
            private readonly string[] _extensions;
            public AllowedExtensionsAttribute(string[] extensions)
            {
                _extensions = extensions;
            }

            protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
            {
                var file = value as IFormFile;
                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }

                return ValidationResult.Success;
            }

            public string GetErrorMessage()
            {
                return $"This photo extension is not allowed!";
            }
        }
        public bool Permission { get; set; }
        public string ProfilePic { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [StringLength(32, ErrorMessage = "The {0} must be at least {2} and not more than {1} characters long.", MinimumLength = 3)]
            [DataType(DataType.Text)]
            [Display(Name = "Username")]
            public string Username { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
            [Display(Name ="Profile picture")]
            [DataType(DataType.Upload)]
            [MaxFileSize(5)]
            [AllowedExtensions(new string[] { ".jpg", ".png" })]
            public IFormFile ProfileImage { get; set; }
        }

        private async Task LoadAsync(AnimuluUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            if (user.ProfilePicExist)
            {
                ProfilePic = user.ProfilePicName+".png";
            }
            else
            {
                ProfilePic = "default.png";
            }
            Input = new InputModel
            {
                Username = userName,
                Password = ""
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/signin");
            }
            await CheckPermissionAsync(user);
            await LoadAsync(user);
            return Page();
        }
        public async Task<IActionResult> OnPostRemovePicAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/signin");
            }
            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            if (!user.ProfilePicExist)
            {
                await LoadAsync(user);
                return Page();
            }
            await CheckPermissionAsync(user);
            string uploadsFolder = Path.Combine(_hostEnv.WebRootPath, "images/avatars");
            string filePath = Path.Combine(uploadsFolder, user.ProfilePicName + ".png");
            System.IO.File.Delete(filePath);
            user.ProfilePicExist = false;
            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile picture has been removed.";
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostPicAsync()
        { 
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/signin");
            }
            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            if (Input.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(_hostEnv.WebRootPath, "images/avatars");
                string filePath = Path.Combine(uploadsFolder, user.ProfilePicName+".png");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Input.ProfileImage.CopyTo(fileStream);
                }
                user.ProfilePicExist = true;
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Your profile picture has been updated.";
            }
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostUsernameAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/signin");
            }
            await CheckPermissionAsync(user);
            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            
            if(Input.Password == null)
            {
                ModelState.AddModelError(string.Empty, "Incorrect password.");
                await LoadAsync(user);
                return Page();
            }

            if (!await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                ModelState.AddModelError(string.Empty, "Incorrect password.");
                await LoadAsync(user);
                return Page();
            }

            var userName = await _userManager.GetUserNameAsync(user);
            if (Input.Username != userName)
            {
                if (await _userManager.FindByNameAsync(Input.Username) == null)
                {
                    var setUsernameResult = await _userManager.SetUserNameAsync(user, Input.Username);
                    if (!setUsernameResult.Succeeded)
                    {
                        StatusMessage = "Unexpected error when trying to set username.";
                        return RedirectToPage();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username is taken.");
                    await LoadAsync(user);
                    return Page();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your username has been updated.";
            return RedirectToPage();
        }
        public async Task CheckPermissionAsync(AnimuluUser user)
        {
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                Permission = true;
            }
            else
            {
                Permission = false;
            }
        }
    }
}
