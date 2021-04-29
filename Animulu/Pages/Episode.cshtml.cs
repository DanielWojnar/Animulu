using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Animulu.Data;
using Animulu.Services;
using Microsoft.EntityFrameworkCore;
using Animulu.Models;

namespace Animulu.Pages
{
    public class EpisodeModel : PageModel
    {
        private readonly ILogger<EpisodeModel> _logger;
        private readonly ShowService _show;
        private readonly EpisodeService _episode;
        private readonly TagService _tag;
        private readonly ViewsService _view;
        public EpisodeModel(ILogger<EpisodeModel> logger, ShowService show, EpisodeService episode, TagService tag, ViewsService view)
        {
            _logger = logger;
            _show = show;
            _episode = episode;
            _tag = tag;
            _view = view;
        }

        public Show showObj;
        public Episode episodeObj;
        public IEnumerable<Episode> Episodes;
        public IEnumerable<Tag> Tags;
        public string Views;

        public async Task<IActionResult> OnGetAsync(string show, int epindex)
        {
            showObj = await _show.GetShowAsync(show);
            Episodes = await _episode.GetEpisodesAsync(showObj);
            episodeObj = await _episode.GetEpisodeAsync(showObj, epindex);
            Tags = await _tag.GetTagsAsync(showObj);
            if (episodeObj.Title == null || episodeObj.Title == "")
            {
                return Redirect("/show/" + show);
            }
            await _view.PostViewAsync(showObj, episodeObj);
            Views = (await _view.GetViewsAsync(episodeObj)).ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de"));
            return Page();
        }
    }
}
