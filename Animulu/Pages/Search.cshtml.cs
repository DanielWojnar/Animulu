using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Animulu.Data;
using Animulu.Models;
using Animulu.Services;

namespace Animulu.Pages
{
    public class SearchModel : PageModel
    {
        private readonly ILogger<SearchModel> _logger;
        private readonly ShowService _show;
        private readonly TagService _tag;
        private readonly NavPageService _navpage;
        public SearchModel(ILogger<SearchModel> logger, ShowService show, TagService tag, NavPageService navpage)
        {
            _logger = logger;
            _show = show;
            _tag = tag;
            _navpage = navpage;
        }

        public IEnumerable<ShowDisplay> Shows;
        public IEnumerable<string> atNames;
        public NavPage navPage;
        public string Quote;
        public string Tag;
        public string Order;
        public async Task<IActionResult> OnGetAsync(string quote, string tag, string order, int pageid)
        {
            atNames = await _tag.GetAllNamesAsync();
            navPage = await _navpage.GetSearchAsync(quote, tag, order, pageid);
            Shows = await _show.GetSearchAsync((navPage.CurPage - 1) * 25, 25, quote, tag, order);
            Quote = quote;
            Order = order;
            Tag = tag;
            return Page();
        }
    }
}
