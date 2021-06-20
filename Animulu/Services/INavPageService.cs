using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Models;

namespace Animulu.Services
{
    public interface INavPageService
    {
        public Task<NavPage> GetPopularAsync(int pageid);
        public Task<NavPage> GetUserRolesAsync(int pageid);
        public Task<NavPage> GetEpisodesAsync(int pageid, int perpage, int showId = 0);
        public Task<NavPage> GetShowsAsync(int pageid, int perpage, string title = null);
        public Task<NavPage> GetSearchAsync(string quote, string tag, string order, int pageid);
    }
}
