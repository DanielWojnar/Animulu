using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Animulu.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Animulu.Areas.Administration.Pages
{
    [Authorize(Policy = "Admin")]
    public class RolesModel : PageModel
    {
        private readonly UserManager<AnimuluUser> _userManager;
        private readonly SignInManager<AnimuluUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RoleService _role;
        private readonly NavPageService _nav;
        public RolesModel(UserManager<AnimuluUser> userManager, SignInManager<AnimuluUser> signInManager, RoleManager<IdentityRole> roleManager, RoleService role, NavPageService nav)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _role = role;
            _nav = nav;
        }
        public NavPage navPage { get; set; }
        public List<TabData> TabDatas { get; set; }
        [BindProperty]
        public RemoveRoleInput UserRoleRemove { get; set; }
        [BindProperty]
        public AddRoleInput UserRoleAdd { get; set; }
        public class RemoveRoleInput{
            public string UserId { get; set; }
            public string RoleName { get; set; }
        }
        public class AddRoleInput
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Username")]
            public string Username { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Role name")]
            public string RoleName { get; set; }
        }
        public class TabData
        {
            public string UserId { get; set; }
            public string Username { get; set; }
            public string RoleName { get; set; }

        }
        public async Task<IActionResult> OnGetAsync(int pageid)
        {
            await LoadDataAsync(pageid);
            return Page();
        }
        public async Task<IActionResult> OnPostRemoveAsync(int pageid)
        {
            if(UserRoleRemove == null)
            {
                await LoadDataAsync(pageid);
                return Page();
            }
            var user = await _userManager.FindByIdAsync(UserRoleRemove.UserId);
            await _userManager.RemoveFromRoleAsync(user, UserRoleRemove.RoleName);
            await LoadDataAsync(pageid);
            return Page();
        }
        public async Task<IActionResult> OnPostAddAsync(int pageid)
        {
            if (UserRoleAdd == null)
            {
                await LoadDataAsync(pageid);
                return Page();
            }
            var user = await _userManager.FindByNameAsync(UserRoleAdd.Username);
            if(user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username.");
            }
            if (!await _roleManager.RoleExistsAsync(UserRoleAdd.RoleName))
            {
                ModelState.AddModelError(string.Empty, "Invalid role name.");
            }
            if (ModelState.IsValid)
            {
                await _userManager.AddToRoleAsync(user, UserRoleAdd.RoleName);
            }
            await LoadDataAsync(pageid);
            return Page();
        }
        public async Task LoadDataAsync(int pageid)
        {
            TabDatas = new List<TabData>();
            var userRoles = await _role.GetAllUsersAsync();
            foreach (var ur in userRoles)
            {
                var user = await _userManager.FindByIdAsync(ur.UserId);
                var role = await _roleManager.FindByIdAsync(ur.RoleId);
                TabDatas.Add(new TabData { UserId = ur.UserId, Username = user.UserName, RoleName = role.Name });
            }
            navPage = await _nav.GetUserRolesAsync(pageid);
        }
    }
}
