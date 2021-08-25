using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Data;
using Animulu.Models;
using Microsoft.EntityFrameworkCore;

namespace Animulu.Services.Implementations
{
    public class TagService : ITagService
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
                var result = await (from t in _context.Tags
                                    join tc in _context.TagConnections on t.Id equals tc.TagId
                                    where tc.ShowId == show.Id
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
