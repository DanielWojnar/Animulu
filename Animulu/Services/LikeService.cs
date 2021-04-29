using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Data;
using Animulu.Models;
using Microsoft.EntityFrameworkCore;

namespace Animulu.Services
{
    public class LikeService
    {
        private readonly AnimuluContext _context;
        public LikeService(AnimuluContext context)
        {
            _context = context;
        }
        private string FormatLike(float count)
        {
            if (count >= 100000000)
            {
                return (count / 1000000).ToString("#M");
            }
            if (count >= 1000000)
            {
                return (count / 1000000).ToString("#.0M");
            }
            if (count >= 100000)
            {
                return (count / 1000).ToString("#K");
            }
            if (count >= 1000)
            {
                return (count / 1000).ToString("#.0K");
            }
            return count.ToString();
        }
        public async Task<Like> GetLikeAsync(Episode episode, AnimuluUser user)
        {
            try
            {
                var result = await (from l in _context.Likes
                                    where l.EpisodeId == episode.Id
                                    where l.UserId == user.Id
                                    select l).FirstAsync();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public async Task<string> GetLikesAsync(Episode episode)
        {
            try
            {
                var count = await (from l in _context.Likes
                                where l.Positive == true
                                where l.EpisodeId == episode.Id
                                select l).AsNoTracking().CountAsync();
                var result = FormatLike(count);
                return result;
            }
            catch
            {
                return "0";
            }
        }
        public async Task<string> GetDislikesAsync(Episode episode)
        {
            try
            {
                var count = await (from l in _context.Likes
                                    where l.Positive == false
                                    where l.EpisodeId == episode.Id
                                    select l).AsNoTracking().CountAsync();
                var result = FormatLike(count);
                return result;
            }
            catch
            {
                return "0";
            }
        }
        public async Task PostLikeAsync(Episode episode, bool positive, AnimuluUser user)
        {
            var like = await GetLikeAsync(episode, user);
            if (like == null)
            {
                await _context.Likes.AddAsync(new Like { EpisodeId = episode.Id, Positive = positive, UserId = user.Id });
                await _context.SaveChangesAsync();
            }
            else
            {
                if(like.Positive == positive)
                {
                    _context.Likes.Remove(like);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    like.Positive = positive;
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
