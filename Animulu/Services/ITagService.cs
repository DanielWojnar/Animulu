using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Models;

namespace Animulu.Services
{
    public interface ITagService
    {
        public Task<IEnumerable<Tag>> GetTagsAsync(Show show);
        public Task<IEnumerable<string>> GetAllNamesAsync();
    }
}
