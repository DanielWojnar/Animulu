using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Data;
using Animulu.Models;
using Microsoft.EntityFrameworkCore;

namespace Animulu.Services.Implementations
{
    public class RatingService : IRatingService
    {
        private readonly AnimuluContext _context;
        public RatingService(AnimuluContext context){
            _context = context;
        }
        
        public async Task<Review> GetRatingAsync(Show show, AnimuluUser user)
        {
            try
            {
                var result = await (from r in _context.Reviews
                                    where r.ShowId == show.Id
                                    where r.UserId == user.Id
                                    select r).FirstAsync();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public async Task<float> GetRatingAsync(Show show)
        {
            try
            {
                var values = await (from r in _context.Reviews
                                    where r.ShowId == show.Id
                                    group r by r.ShowId into g
                                    select new { Count = g.Count(), Sum = g.Sum(x => x.Value) }).AsNoTracking().FirstAsync();

                var result = (float)values.Sum / (float)values.Count;
                if(float.IsNaN(result))
                {
                    result = 0f;
                }
                return result;
            }
            catch
            {
                return 0f;
            }
        }
        public async Task<int> GetRatingAmoutAsync(Show show)
        {
            var result = await (from r in _context.Reviews
                                where r.ShowId == show.Id
                                select r).AsNoTracking().CountAsync();
            return result;
        }
        public async Task PostRatingAsync(Show show, int score, AnimuluUser user)
        {
            var rate = await GetRatingAsync(show, user);
            if(rate == null)
            {
                await _context.Reviews.AddAsync(new Review { ShowId = show.Id, UserId = user.Id, Value = score });
                await _context.SaveChangesAsync();
            }
            else
            {
                rate.Value = score;
                await _context.SaveChangesAsync();
            }
        }
    }
}
