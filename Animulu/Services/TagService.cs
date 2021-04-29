using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Data;
using Animulu.Models;
using Microsoft.EntityFrameworkCore;

namespace Animulu.Services
{
    public class TagService
    {
        private readonly AnimuluContext _context;
        public TagService(AnimuluContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Tag>> GetTagsAsync(Show show)
        {
            try
            {
                var tcs = from tc in _context.TagConnections
                          where tc.ShowId == show.Id
                          select tc;
                var result = await (from t in _context.Tags
                                    join tc in tcs on t.Id equals tc.TagId
                                    select t).AsNoTracking().ToListAsync();
                return result;
            }
            catch
            {
                return new List<Tag>();
            }
        }
        public async Task<IEnumerable<string>> GetAllNamesAsync()
        {
            try
            {
                var result = await (from at in _context.Tags
                                    orderby at.Name
                                    select at.Name).AsNoTracking().ToListAsync();
                return result;
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}
