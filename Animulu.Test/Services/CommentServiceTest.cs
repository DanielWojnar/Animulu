using System;
using Xunit;
using Animulu.Services.Implementations;
using Animulu.Models;
using Animulu.Data;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Animulu.Test.Services
{
    public class CommentServiceTest : AnimuluContextTestBase
    {
        private readonly Comment[] _comments;
        public CommentServiceTest()
        {
            _comments = AnimuluContextTestInitializer.comments;
        }

        [Theory]
        [InlineData(3)]
        [InlineData(1)]
        [InlineData(4)]
        public async void GetCommentAsync_ShouldWork(int commentId)
        {
            var service = new CommentService(_context, null);
            var actual = await service.GetCommentAsync(commentId);
            Assert.Equal(commentId, actual.Id);
        }

        [Theory]
        [InlineData(15)]
        [InlineData(31)]
        [InlineData(63)]
        public async void GetCommentAsync_ShouldNotWork(int commentId)
        {
            var service = new CommentService(_context, null);
            var actual = await service.GetCommentAsync(commentId);
            Assert.Null(actual);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(4, 1)]
        [InlineData(5, 4)]
        [InlineData(10, 0)]
        public async void GetCommentCountAsync_ShouldWork(int episodeId, int expected)
        {
            var service = new CommentService(_context, null);
            var actual = await service.GetCommentCountAsync(new Episode() { Id = episodeId });
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "abc", "some content")]
        [InlineData(3, "gzd", "anything")]
        [InlineData(5, "gzd", "anything")]
        public async void PostCommentAsync_ShouldWork(int episodeId, string userId, string content)
        {
            var service = new CommentService(_context, null);
            var expected = await service.GetCommentCountAsync(new Episode() { Id = episodeId }) + 1;
            await service.PostCommentAsync(new Episode() { Id = episodeId }, userId, content);
            var actual = await service.GetCommentCountAsync(new Episode() { Id = episodeId });
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(4, 3)]
        [InlineData(9, 5)]
        public async void RemoveCommentAsync_ShouldWork(int commentId, int episodeId)
        {
            var service = new CommentService(_context, null);
            var expected = await service.GetCommentCountAsync(new Episode() { Id = episodeId }) - 1;
            await service.RemoveCommentAsync(commentId);
            var actual = await service.GetCommentCountAsync(new Episode() { Id = episodeId });
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("3")]
        [InlineData("4")]
        [InlineData("100")]
        public async void RemoveUserCommentsAsync_ShouldWork(string userId)
        {
            var service = new CommentService(_context, null);
            await service.RemoveUserCommentsAsync(userId);
            var actuals = await (from c in _context.Comments
                                 select c).AsNoTracking().ToListAsync();
            foreach (var com in actuals)
            {
                Assert.NotEqual(userId, com.UserId); 
            }
        }
    }
}
