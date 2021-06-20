using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Models;

namespace Animulu.Services
{
    public interface ICommentService
    {
        public Task RemoveUserCommentsAsync(string userId);
        public Task RemoveCommentAsync(int commentid);
        public Task<Comment> GetCommentAsync(int commentid);
        public Task<IEnumerable<CommentDisplay>> GetCommentsAsync(Episode episode, int take);
        public Task PostCommentAsync(Episode episode, string userId, string content);
        public Task<int> GetCommentCountAsync(Episode episode);
    }
}
