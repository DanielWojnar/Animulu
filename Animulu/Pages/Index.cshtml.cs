using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Data;
using Animulu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Animulu.Services;
namespace Animulu.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IShowService _show;
        private readonly IEpisodeService _episode;
        private readonly ITagService _tag;
        public IndexModel(ILogger<IndexModel> logger, IShowService show, IEpisodeService episode, ITagService tag)
        {
            _logger = logger;
            _show = show;
            _episode = episode;
            _tag = tag;
        }

        public IEnumerable<ShowDisplay> RandomShows;
        public IEnumerable<EpisodeDisplay> UploadEpisodes;
        public IEnumerable<EpisodeDisplay> ReleasedEpisodes;
        public IEnumerable<ShowDisplay> PopularShows;
        public IEnumerable<Tag> FeaturedTags;
        public Show FeaturedShow;

        public async Task<IActionResult> OnGetAsync()
        {
            RandomShows = await _show.GetRandomAsync(0, 30);
            ReleasedEpisodes = await _episode.GetReleasedAsync(0,30);
            UploadEpisodes = await _episode.GetUploadedAsync(0,30);
            PopularShows = await _show.GetPopularAsync(0,30);
            FeaturedShow = await _show.GetFeaturedAsync();
            FeaturedTags = await _tag.GetTagsAsync(FeaturedShow);
            return Page();
        }
    }
}
