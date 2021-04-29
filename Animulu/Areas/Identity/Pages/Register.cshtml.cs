using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Animulu.Data;
using Animulu.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Animulu.Areas.Identity.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AnimuluUser> _signInManager;
        private readonly UserManager<AnimuluUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(UserManager<AnimuluUser> userManager, SignInManager<AnimuluUser> signInManager, ILogger<RegisterModel> logger, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(32, ErrorMessage = "The {0} must be at least {2} and not more than {1} characters long.", MinimumLength = 3)]
            [DataType(DataType.Text)]
            [Display(Name = "Username")]
            public string Username { get; set; }
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and not more than {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The passwords do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public IActionResult OnGet(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = returnUrl;
            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect(ReturnUrl);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect(returnUrl);
            }
            if (ModelState.IsValid)
            {
                var user = new AnimuluUser { UserName = Input.Username, Email = Input.Email };
                if(await _userManager.FindByEmailAsync(user.Email) != null )
                {
                    ModelState.AddModelError(string.Empty,"Email is taken.");
                    return Page();
                }
                if(await _userManager.FindByNameAsync(user.UserName) != null)
                {
                    ModelState.AddModelError(string.Empty, "Username is taken.");
                    return Page();
                }
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your account on Animulu",
                        $"Hello {Input.Username}, please confirm your Animulu account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
