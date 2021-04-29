using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Animulu.Data;
using Animulu.Models;

namespace Animulu.Areas.Identity.Pages.Account
{
    public partial class EmailModel : PageModel
    {
        private readonly UserManager<AnimuluUser> _userManager;
        private readonly SignInManager<AnimuluUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public EmailModel(UserManager<AnimuluUser> userManager, SignInManager<AnimuluUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public bool Permission { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
        }

        private async Task LoadAsync(AnimuluUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = "",
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

        public async Task<IActionResult> OnPostChangeEmailAsync()
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

            if (!await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                ModelState.AddModelError(string.Empty, "Incorrect password.");
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                if (await _userManager.FindByEmailAsync(Input.NewEmail) == null)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/ConfirmEmailChange",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, email = Input.NewEmail, code = code },
                        protocol: Request.Scheme);
                    await _emailSender.SendEmailAsync(
                        Input.NewEmail,
                        "Confirm your new email",
                        $"Please confirm your Animulu account new email by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    StatusMessage = "Confirmation link to change email sent. Please check your email.";
                    return RedirectToPage();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email is taken.");
                    await LoadAsync(user);
                    return Page();
                }
            }

            StatusMessage = "Your email is unchanged.";
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