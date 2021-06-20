using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Models;

namespace Animulu.Services
{
    public interface IViewsService
    {
        public Task PostViewAsync(Show show, Episode episode);
        public Task<int> GetViewsAsync(Episode episode);
        public Task<int> GetViewsAsync(Show show);
    }
}
