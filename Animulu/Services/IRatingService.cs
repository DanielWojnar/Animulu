using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Models;

namespace Animulu.Services
{
    public interface IRatingService
    {
        public Task<Review> GetRatingAsync(Show show, AnimuluUser user);
        public Task<float> GetRatingAsync(Show show);
        public Task<int> GetRatingAmoutAsync(Show show);
        public Task PostRatingAsync(Show show, int score, AnimuluUser user);
    }
}
