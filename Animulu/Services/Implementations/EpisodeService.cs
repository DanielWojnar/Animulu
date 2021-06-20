using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Data;
using Animulu.Models;
using Microsoft.EntityFrameworkCore;

namespace Animulu.Services.Implementations
{
    public class EpisodeService : IEpisodeService
    {
        private readonly AnimuluContext _context;
        public EpisodeService(AnimuluContext context)
        {
            _context = context;
        }
        public async Task RemoveEpisodeAsync(int id)
        {
            var episode = await GetEpisodeAsync(id);
            if (episode == null)
            {
                return;
            }
            _context.Episodes.Remove(episode);
            await _context.SaveChangesAsync();
        }
        public async Task PostEpisodeAsync(Episode episode)
        {
            await _context.Episodes.AddAsync(episode);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<EpisodeDisplay>> GetReleasedAsync(int skip, int take)
        {
            try
            {
                var views = from v in _context.Views
                            group v.EpisodeId by v.EpisodeId into g
                            select new { EpisdeId = g.Key, Count = g.Count() };
                var result = await (from e in _context.Episodes
                                    join s in _context.Shows on e.ShowId equals s.Id
                                    join v in views on e.Id equals v.EpisdeId into EpisodeGroup
                                    from eg in EpisodeGroup.DefaultIfEmpty()
                                    orderby e.ReleaseDate descending
                                    select new EpisodeDisplay
                                    {
                                        Id = e.Id,
                                        ShowId = e.ShowId,
                                        EpisodeIndex = e.EpisodeIndex,
                                        CoverImg = s.CoverImg,
                                        ShowTitle = s.Title,
                                        Views = eg == null ? 0 : eg.Count
                                    }).Skip(skip).Take(take).AsNoTracking().ToListAsync();
                return result;
            }
            catch
            {
                return new List<EpisodeDisplay>();
            }
        }
        public async Task<IEnumerable<EpisodeDisplay>> GetUploadedAsync(int skip, int take) 
        {
            try
            {
                var views = from v in _context.Views
                            group v.EpisodeId by v.EpisodeId into g
                            select new { EpisdeId = g.Key, Count = g.Count() };
                var result = await (from e in _context.Episodes
                                    join s in _context.Shows on e.ShowId equals s.Id
                                    join v in views on e.Id equals v.EpisdeId into EpisodeGroup
                                    from eg in EpisodeGroup.DefaultIfEmpty()
                                    orderby e.UploadDate descending
                                    select new EpisodeDisplay
                                    {
                                        Id = e.Id,
                                        ShowId = e.ShowId,
                                        EpisodeIndex = e.EpisodeIndex,
                                        CoverImg = s.CoverImg,
                                        ShowTitle = s.Title,
                                        Views = eg == null ? 0 : eg.Count
                                    }).Skip(skip).Take(take).AsNoTracking().ToListAsync();
                return result;
            }
            catch
            {
                return new List<EpisodeDisplay>();
            }
        }
        public async Task<List<Episode>> GetEpisodesAsync(int showId = 0, int skip = 0, int take = 0)
        {
            try
            {
                var query = from e in _context.Episodes
                            select e;
                if(showId != 0)
                {
                    query = query.Where(e => e.ShowId == showId);
                }
                if(skip != 0)
                {
                    query = query.Skip(skip);
                }
                if(take != 0)
                {
                    query = query.Take(take);
                }
                var result = await query.AsNoTracking().ToListAsync();
                return result;
            }
            catch
            {
                return new List<Episode>();
            }
        }
        public async Task<IEnumerable<Episode>> GetEpisodesAsync(Show show)
        {
            try
            {
                var result = await (from e in _context.Episodes
                                    where e.ShowId == show.Id
                                    orderby e.EpisodeIndex
                                    select e).AsNoTracking().ToListAsync();
                return result;
            }
            catch
            {
                return new List<Episode>();
            }
        }
        public async Task<Episode> GetEpisodeAsync(int id)
        {
            try
            {
                var result = await (from e in _context.Episodes
                                    where e.Id == id
                                    select e).AsNoTracking().FirstAsync();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public async Task<Episode> GetEpisodeAsync(Show show,int index)
        {
            try
            {
                var result = await (from e in _context.Episodes
                                    where e.ShowId == show.Id
                                    where e.EpisodeIndex == index
                                    select e).AsNoTracking().FirstAsync();
                return result;
            }
            catch
            {
                return new Episode();
            }
        }
    }
}
