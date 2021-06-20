using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Models;

namespace Animulu.Services
{
    public interface ILikeService
    {
        public Task<Like> GetLikeAsync(Episode episode, AnimuluUser user);
        public Task<string> GetLikesAsync(Episode episode);
        public Task<string> GetDislikesAsync(Episode episode);
        public Task PostLikeAsync(Episode episode, bool positive, AnimuluUser user);
    }
}
