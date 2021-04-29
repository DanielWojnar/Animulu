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
        private readonly ShowService _show;
        private readonly EpisodeService _episode;
        private readonly TagService _tag;
        private readonly RatingService _rating;
        public ShowModel(ILogger<ShowModel> logger, ShowService show, EpisodeService episode, TagService tag, RatingService rating)
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
            episodes = await _episode.GetEpisodesAsync(showObj);
            Tags = await _tag.GetTagsAsync(showObj);
            if(showObj.Title == "" || showObj.Title == null)
            {
                return Redirect("/search");
            }
            return Page();
        }
    }
}
