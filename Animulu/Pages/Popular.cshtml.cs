using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Animulu.Data;
using Animulu.Models;
using Animulu.Services;
using Microsoft.EntityFrameworkCore;

namespace Animulu.Pages
{
    public class PopularModel : PageModel
    {
        private readonly ILogger<PopularModel> _logger;
        private readonly IShowService _show;
        private readonly INavPageService _navpage;
        public PopularModel(ILogger<PopularModel> logger, IShowService show, INavPageService navpage)
        {
            _logger = logger;
            _show = show;
            _navpage = navpage;
        }

        public IEnumerable<ShowDisplay> Shows;
        public NavPage navPage;
        public async Task<IActionResult> OnGetAsync(int pageid)
        {
            navPage = await _navpage.GetPopularAsync(pageid);
            Shows = await _show.GetPopularAsync((navPage.CurPage - 1) * 25, 25);
            return Page();
        }
    }
}
