using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Models;
using Animulu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Animulu.Areas.Administration.Pages
{
    [Authorize(Policy = "Admin")]
    public class EpisodesModel : PageModel
    {
        private readonly EpisodeService _episode;
        private readonly NavPageService _nav;
        public EpisodesModel(EpisodeService episode, NavPageService nav)
        {
            _episode = episode;
            _nav = nav;
        }
        public int SearchShowId { get; set; }
        public NavPage navPage { get; set; }
        public List<Episode> TabDatas { get; set; }
        [BindProperty]
        public int EpisodeRemoveId { get; set; }
        [BindProperty]
        public AddEpisodeInput EpisodeAdd { get; set; }
        public class AddEpisodeInput
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Show Id")]
            public int ShowIdEp { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Episode Index")]
            public int EpisodeIndex { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Title")]
            public string EpisodeTitle { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Video Source")]
            public string VideoSrc { get; set; }
            [DataType(DataType.Text)]
            [Display(Name = "Description")]
            public string Description { get; set; }
            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Upload date")]
            public DateTime UploadDate { get; set; }
            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Release date")]
            public DateTime ReleaseDate { get; set; }
        }
        public async Task<IActionResult> OnGetAsync(int pageid, int showid = 0)
        {
            SearchShowId = showid;
            await LoadDataAsync(showid, pageid);
            return Page();
        }
        public async Task<IActionResult> OnPostAddAsync(int pageid, int showid = 0)
        {
            SearchShowId = showid;
            if (!ModelState.IsValid)
            {
                await LoadDataAsync(showid, pageid);
                return Page();
            }
            await _episode.PostEpisodeAsync(new Episode { ShowId = EpisodeAdd.ShowIdEp, EpisodeIndex = EpisodeAdd.EpisodeIndex, VideoSrc = EpisodeAdd.VideoSrc, Title = EpisodeAdd.EpisodeTitle, Description = EpisodeAdd.Description, ReleaseDate = EpisodeAdd.ReleaseDate, UploadDate = EpisodeAdd.UploadDate });
            await LoadDataAsync(showid, pageid);
            return Page();
        }
        public async Task<IActionResult> OnPostRemoveAsync(int pageid, int showid = 0)
        {
            SearchShowId = showid;
            await _episode.RemoveEpisodeAsync(EpisodeRemoveId);
            await LoadDataAsync(showid, pageid);
            return Page();
        }
        public async Task LoadDataAsync(int showid, int pageid)
        {
            TabDatas = await _episode.GetEpisodesAsync(showid, (pageid-1)*200, 200);
            navPage = await _nav.GetEpisodesAsync(pageid, 200, showid);
        }
    }
}
