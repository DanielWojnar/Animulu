using System;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Animulu.Data;
using Animulu.Models;
using Animulu.Services;

namespace Animulu.Areas.Identity.Pages
{
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<AnimuluUser> _userManager;

        public RegisterConfirmationModel(UserManager<AnimuluUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            return Page();
        }
    }
}
