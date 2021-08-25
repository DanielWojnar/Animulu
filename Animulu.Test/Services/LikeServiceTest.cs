using System;
using Xunit;
using Animulu.Services.Implementations;
using Animulu.Models;
using Animulu.Data;


namespace Animulu.Test.Services
{
    public class LikeServiceTest : AnimuluContextTestBase
    {
        private readonly Like[] _likes;
        public LikeServiceTest()
        {
            _likes = AnimuluContextTestInitializer.likes;
        }
        [Theory]
        [InlineData(1, "1", 1)]
        [InlineData(4, "3", 3)]
        [InlineData(2, "4", 2)]
        public async void GetLikeAsync_ShouldWork(int episodeId, string userId, int expected)
        {
            var service = new LikeService(_context);
            var actual = await service.GetLikeAsync(new Episode() { Id = episodeId }, new AnimuluUser() { Id = userId });
            Assert.Equal(expected, actual.Id);
        }

        [Theory]
        [InlineData(10, "21")]
        [InlineData(10, "30")]
        [InlineData(36, "1")]
        public async void GetLikeAsync_ShouldNotWork(int episodeId, string userId)
        {
            var service = new LikeService(_context);
            var actual = await service.GetLikeAsync(new Episode() { Id = episodeId }, new AnimuluUser() { Id = userId });
            Assert.Null(actual);
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        [InlineData(5, "4")]
        [InlineData(8, "0")]
        public async void GetLikesAsync_ShouldWork(int episodeId, string expected)
        {
            var service = new LikeService(_context);
            var actual = await service.GetLikesAsync(new Episode() { Id = episodeId });
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(8, "2")]
        [InlineData(2, "1")]
        [InlineData(15, "0")]
        public async void GetDislikesAsync_ShouldWork(int episodeId, string expected)
        {
            var service = new LikeService(_context);
            var actual = await service.GetDislikesAsync(new Episode() { Id = episodeId });
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, true, "user1")]
        [InlineData(4, true, "user3")]
        [InlineData(14, true, "user2")]
        [InlineData(18, false, "user3")]
        [InlineData(30, false, "userA")]
        [InlineData(31, false, "userB")]
        public async void PostLikeAsync_ShouldAddNewLike(int episodeId, bool positive, string userId)
        {
            var service = new LikeService(_context);
            var expected = "0";
            if (positive)
            {
                expected = await service.GetLikesAsync(new Episode() { Id = episodeId });
            }
            else
            {
                expected = await service.GetDislikesAsync(new Episode() { Id = episodeId });
            }
            expected = (int.Parse(expected) + 1).ToString();
            await service.PostLikeAsync(new Episode() { Id = episodeId }, positive, new AnimuluUser() { Id = userId });
            var actual = "0";
            if (positive)
            {
                actual = await service.GetLikesAsync(new Episode() { Id = episodeId });
            }
            else
            {
                actual = await service.GetDislikesAsync(new Episode() { Id = episodeId });
            }
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, true, "1")]
        [InlineData(4, true, "3")]
        [InlineData(5, true, "6")]
        [InlineData(8, false, "azd")]
        [InlineData(8, false, "zxy")]
        [InlineData(2, false, "yxz")]
        public async void PostLikeAsync_ShouldRemoveLike(int episodeId, bool positive, string userId)
        {
            var service = new LikeService(_context);
            var expected = "0";
            if (positive)
            {
                expected = await service.GetLikesAsync(new Episode() { Id = episodeId });
            }
            else
            {
                expected = await service.GetDislikesAsync(new Episode() { Id = episodeId });
            }
            expected = (int.Parse(expected) - 1).ToString();
            await service.PostLikeAsync(new Episode() { Id = episodeId }, positive, new AnimuluUser() { Id = userId });
            var actual = "0";
            if (positive)
            {
                actual = await service.GetLikesAsync(new Episode() { Id = episodeId });
            }
            else
            {
                actual = await service.GetDislikesAsync(new Episode() { Id = episodeId });
            }
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, false, "1")]
        [InlineData(4, false, "3")]
        [InlineData(5, false, "6")]
        [InlineData(8, true, "azd")]
        [InlineData(8, true, "zxy")]
        [InlineData(2, true, "yxz")]
        public async void PostLikeAsync_ShouldChangeLike(int episodeId, bool positive, string userId)
        {
            var service = new LikeService(_context);
            var expectedLikes = await service.GetLikesAsync(new Episode() { Id = episodeId });
            var expectedDislikes = await service.GetDislikesAsync(new Episode() { Id = episodeId });
            var expectedPositive = (await service.GetLikeAsync(new Episode() { Id = episodeId }, new AnimuluUser() { Id = userId })).Positive;
            if (positive)
            {
                expectedLikes = (int.Parse(expectedLikes) + 1).ToString();
                expectedDislikes = (int.Parse(expectedDislikes) - 1).ToString();
            }
            else
            {
                expectedLikes = (int.Parse(expectedLikes) - 1).ToString();
                expectedDislikes = (int.Parse(expectedDislikes) + 1).ToString();
            }
            await service.PostLikeAsync(new Episode() { Id = episodeId }, positive, new AnimuluUser() { Id = userId });
            var actualLikes = await service.GetLikesAsync(new Episode() { Id = episodeId });
            var actualDislikes = await service.GetDislikesAsync(new Episode() { Id = episodeId });
            var actualPositive = (await service.GetLikeAsync(new Episode() { Id = episodeId }, new AnimuluUser() { Id = userId })).Positive;
            Assert.Equal(expectedLikes, actualLikes);
            Assert.Equal(expectedDislikes, actualDislikes);
            Assert.NotEqual(expectedPositive, actualPositive);
        }
    }
}
