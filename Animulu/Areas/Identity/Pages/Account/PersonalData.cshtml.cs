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

namespace Animulu.Areas.Identity.Pages.Account
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<AnimuluUser> _userManager;

        public PersonalDataModel(UserManager<AnimuluUser> userManager)
        {
            _userManager = userManager;
        }
        public bool Permission { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("/signin");
            }
            await CheckPermissionAsync(user);
            return Page();
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
