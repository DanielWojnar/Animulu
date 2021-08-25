using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Data;
using Animulu.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Animulu.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly AnimuluContext _context;
        private readonly UserManager<AnimuluUser> _userManager;
        public CommentService(AnimuluContext context, UserManager<AnimuluUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task RemoveUserCommentsAsync(string userId)
        {
            _context.Comments.RemoveRange(_context.Comments.Where(c => c.UserId == userId));
            await _context.SaveChangesAsync();
        }
        public async Task RemoveCommentAsync(int commentid)
        {
            var comment = new Comment { Id = commentid };
            _context.Comments.Attach(comment);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
        public async Task<Comment> GetCommentAsync(int commentid)
        {
            try
            {
                var result = await (from c in _context.Comments
                                    where c.Id == commentid
                                    select c).AsNoTracking().FirstAsync();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public async Task<IEnumerable<CommentDisplay>> GetCommentsAsync(Episode episode, int take)
        {
            try
            {
                var comments = await (from c in _context.Comments
                                     where c.EpisodeId == episode.Id
                                     orderby c.CreationDate descending
                                     select c).Take(take+1).AsNoTracking().ToListAsync();
                var result = await ConvertCommentAsync(comments);
                return result;
            }
            catch
            {
                return new List<CommentDisplay>();
            }
        }
        public async Task PostCommentAsync(Episode episode, string userId, string content)
        {
            await _context.Comments.AddAsync(new Comment {EpisodeId=episode.Id, UserId=userId, Content=content, CreationDate=DateTime.Now});
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetCommentCountAsync(Episode episode)
        {
            try
            {
                var result = await (from c in _context.Comments
                                    where c.EpisodeId == episode.Id
                                    orderby c.CreationDate descending
                                    select c).AsNoTracking().CountAsync();
                return result;
            }
            catch
            {
                return 0;
            }
        }
        async Task<IEnumerable<CommentDisplay>> ConvertCommentAsync(IEnumerable<Comment> comments)
        {
            List<CommentDisplay> result = new List<CommentDisplay>();
            foreach(var c in comments)
            {
                var user = await _userManager.FindByIdAsync(c.UserId);
                var username = user.UserName;
                var profilePic = user.ProfilePicExist ? user.ProfilePicName+".png" : "default.png";
                result.Add(new CommentDisplay {Id = c.Id, Content = c.Content, Username = username, ProfilePic = profilePic, CreationDate = c.CreationDate });
            }
            return result;
        }
    }
}
