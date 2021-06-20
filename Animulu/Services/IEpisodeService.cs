using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Models;

namespace Animulu.Services
{
    public interface IEpisodeService
    {
        public Task RemoveEpisodeAsync(int id);
        public Task PostEpisodeAsync(Episode episode);
        public Task<IEnumerable<EpisodeDisplay>> GetReleasedAsync(int skip, int take);
        public Task<IEnumerable<EpisodeDisplay>> GetUploadedAsync(int skip, int take);
        public Task<List<Episode>> GetEpisodesAsync(int showId = 0, int skip = 0, int take = 0);
        public Task<IEnumerable<Episode>> GetEpisodesAsync(Show show);
        public Task<Episode> GetEpisodeAsync(int id);
        public Task<Episode> GetEpisodeAsync(Show show, int index);
    }
}
