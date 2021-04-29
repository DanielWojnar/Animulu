using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Animulu.Data;
using Animulu.Models;

namespace Animulu.Services
{
    public class ViewsService
    {
        private readonly AnimuluContext _context;
        public ViewsService(AnimuluContext context)
        {
            _context = context;
        }
        public async Task PostViewAsync(Show show, Episode episode)
        {
            await _context.Views.AddAsync(new View {EpisodeId=episode.Id,ShowId=show.Id,ViewDate = DateTime.Now});
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetViewsAsync(Episode episode)
        {
            try
            {
                var result = await (from v in _context.Views
                                where v.EpisodeId == episode.Id
                                select v).AsNoTracking().CountAsync();
                return result;
            }
            catch
            {
                return 0;
            }
        }
        public async Task<int> GetViewsAsync(Show show)
        {
            try
            {
                var result = await (from v in _context.Views
                                    where v.ShowId == show.Id
                                    select v).AsNoTracking().CountAsync();
                return result;
            }
            catch
            {
                return 0;
            }
        }
    }
}
