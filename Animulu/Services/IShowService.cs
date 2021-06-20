using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Models;

namespace Animulu.Services
{
    public interface IShowService
    {
        public Task RemoveShowAsync(int id);
        public Task PostShowAsync(Show show);
        public Task<List<Show>> GetShowsAsync(string queryTitle = null, int skip = 0, int take = 0);
        public Task<IEnumerable<ShowDisplay>> GetPopularAsync(int skip, int take);
        public Task<IEnumerable<ShowDisplay>> GetRandomAsync(int skip, int take);
        public Task<Show> GetFeaturedAsync();
        public Task<Show> GetShowAsync(int id);
        public Task<Show> GetShowAsync(string name);
        public Task<IEnumerable<ShowDisplay>> GetSearchAsync(int skip, int take, string quote, string tag, string order);
    }
}
