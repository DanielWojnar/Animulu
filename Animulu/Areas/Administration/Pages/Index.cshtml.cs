using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Animulu.Services;
using Animulu.Models;

namespace Animulu.Areas.Administration.Pages
{
    [Authorize(Policy = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IShowService _show;
        private readonly INavPageService _nav;
        public IndexModel(IShowService show, INavPageService nav)
        {
            _show = show;
            _nav = nav;
        }
        public string Title { get; set; }
        public NavPage navPage { get; set; }
        public List<Show> TabDatas { get; set; }
        [BindProperty]
        public int ShowRemoveId { get; set; }
        [BindProperty]
        public AddShowInput ShowAdd { get; set; }
        public class AddShowInput
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Title")]
            public string ShowTitle { get; set; }
            [DataType(DataType.Text)]
            [Display(Name = "Description")]
            public string Description { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Cover")]
            public string Cover { get; set; }
            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Release date")]
            public DateTime ReleaseDate { get; set; }
        }
        public async Task<IActionResult> OnGetAsync(int pageid, string title = null)
        {
            Title = title;
            await LoadDataAsync(title, pageid);
            return Page();
        }
        public async Task<IActionResult> OnPostAddAsync(int pageid, string title = null)
        {
            Title = title;
            if (!ModelState.IsValid)
            {
                await LoadDataAsync(title, pageid);
                return Page();
            }
            await _show.PostShowAsync(new Show { Title = ShowAdd.ShowTitle, Description = ShowAdd.Description, CoverImg = ShowAdd.Cover, ReleaseDate = ShowAdd.ReleaseDate });
            await LoadDataAsync(title, pageid);
            return Page();
        }
        public async Task<IActionResult> OnPostRemoveAsync(int pageid, string title = null)
        {
            Title = title;
            await _show.RemoveShowAsync(ShowRemoveId);
            await LoadDataAsync(title, pageid);
            return Page();
        }
        public async Task LoadDataAsync(string title, int pageid)
        {
            TabDatas = await _show.GetShowsAsync(Title, (pageid - 1)*200, 200);
            navPage = await _nav.GetShowsAsync(pageid, 200, title);
        }
    }
}
