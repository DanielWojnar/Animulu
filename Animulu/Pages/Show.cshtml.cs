using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Animulu.Data;
using Animulu.Models;
using Animulu.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Animulu.Pages
{
    public class ShowModel : PageModel
    {
        private readonly ILogger<ShowModel> _logger;
        private readonly IShowService _show;
        private readonly IEpisodeService _episode;
        private readonly ITagService _tag;
        private readonly IRatingService _rating;
        public ShowModel(ILogger<ShowModel> logger, IShowService show, IEpisodeService episode, ITagService tag, IRatingService rating)
        {
            _logger = logger;
            _show = show;
            _episode = episode;
            _tag = tag;
            _rating = rating;
        }

        public Show showObj;
        public IEnumerable<Episode> episodes;
        public IEnumerable<Tag> Tags;

        public async Task<IActionResult> OnGetAsync(string show)
        {
            showObj = await _show.GetShowAsync(show);
            if(showObj == null)
            {
                return Redirect("/search");
            }
            episodes = await _episode.GetEpisodesAsync(showObj);
            Tags = await _tag.GetTagsAsync(showObj);
            return Page();
        }
    }
}
