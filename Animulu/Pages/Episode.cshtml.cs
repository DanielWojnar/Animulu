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
        private readonly IShowService _show;
        private readonly IEpisodeService _episode;
        private readonly ITagService _tag;
        private readonly IViewsService _view;
        public EpisodeModel(ILogger<EpisodeModel> logger, IShowService show, IEpisodeService episode, ITagService tag, IViewsService view)
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
            if(showObj == null)
            {
                return Redirect("/search");
            }
            Episodes = await _episode.GetEpisodesAsync(showObj);
            episodeObj = await _episode.GetEpisodeAsync(showObj, epindex);
            if(episodeObj == null)
            {
                return Redirect("/show/" + show);
            }
            Tags = await _tag.GetTagsAsync(showObj);
            await _view.PostViewAsync(showObj, episodeObj);
            Views = (await _view.GetViewsAsync(episodeObj)).ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de"));
            return Page();
        }
    }
}
