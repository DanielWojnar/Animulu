using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Animulu.Data;
using Animulu.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Animulu.Services;

namespace Animulu.Areas.Identity.Pages.Account
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<AnimuluUser> _userManager;
        private readonly SignInManager<AnimuluUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly CommentService _comment;

        public DeletePersonalDataModel(UserManager<AnimuluUser> userManager, SignInManager<AnimuluUser> signInManager, ILogger<DeletePersonalDataModel> logger, IWebHostEnvironment hostEnv, CommentService comment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _hostEnv = hostEnv;
            _comment = comment;
        }
        public bool Permission { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/signin");
            }
            await CheckPermissionAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/signin");
            }
            await CheckPermissionAsync(user);
            if (!await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                ModelState.AddModelError(string.Empty, "Incorrect password.");
                return Page();
            }
            
            var userId = await _userManager.GetUserIdAsync(user);
            if (user.ProfilePicExist)
            {
                string uploadsFolder = Path.Combine(_hostEnv.WebRootPath, "images/avatars");
                string filePath = Path.Combine(uploadsFolder, user.ProfilePicName + ".png");
                System.IO.File.Delete(filePath);
            }
            await _comment.RemoveUserCommentsAsync(userId);
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
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
