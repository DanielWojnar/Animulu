using System;
using Xunit;
using Animulu.Services.Implementations;
using Animulu.Models;
using Animulu.Data;

namespace Animulu.Test.Services
{
    public class RatingServiceTest : AnimuluContextTestBase
    {
        private readonly Review[] _reviews;
        public RatingServiceTest()
        {
            _reviews = AnimuluContextTestInitializer.reviews;
        }

        [Theory]
        [InlineData(4, "1", 1)]
        [InlineData(5, "2", 6)]
        [InlineData(10, "11", null)]
        public async void GetRatingAsync_forUser_ShouldWork(int showId, string userId, int? reviewId)
        {
            var service = new RatingService(_context);
            var actual = await service.GetRatingAsync(new Show() { Id = showId }, new AnimuluUser() { Id = userId });
            if(actual != null)
            {
                Assert.Equal(reviewId, actual.Id);
            }
            else
            {
                Assert.Null(reviewId);
                Assert.Null(actual);
            }
        }

        [Theory]
        [InlineData(4, 4.75)]
        [InlineData(5, 1.5)]
        [InlineData(10, 0)]
        public async void GetRatingAsync_forShow_ShouldWork(int showId, float expected)
        {
            var service = new RatingService(_context);
            var actual = await service.GetRatingAsync(new Show() { Id = showId });
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(4, 4)]
        [InlineData(5, 2)]
        [InlineData(10, 0)]
        public async void GetRatingAmoutAsync_ShouldWork(int showId, int expected)
        {
            var service = new RatingService(_context);
            var actual = await service.GetRatingAmoutAsync(new Show() { Id = showId });
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(4, 1, "11")]
        [InlineData(5, 2, "12")]
        [InlineData(10, 4, "13")]
        public async void PostRatingAsync_ShouldAddNewReview(int showId, int score, string userId)
        {
            var service = new RatingService(_context);
            var expectedCount = await service.GetRatingAmoutAsync(new Show() { Id = showId }) + 1;
            await service.PostRatingAsync(new Show() { Id = showId }, score, new AnimuluUser() { Id = userId });
            var actualCount = await service.GetRatingAmoutAsync(new Show() { Id = showId });
            var acutalScore = (await service.GetRatingAsync(new Show() { Id = showId }, new AnimuluUser() { Id = userId })).Value;
            Assert.Equal(expectedCount, actualCount);
            Assert.Equal(score, acutalScore);
        }

        [Theory]
        [InlineData(4, 1, "1")]
        [InlineData(5, 4, "2")]
        [InlineData(5, 1, "1")]
        public async void PostRatingAsync_ShouldEditOldReview(int showId, int score, string userId)
        {
            var service = new RatingService(_context);
            var expectedCount = await service.GetRatingAmoutAsync(new Show() { Id = showId });
            await service.PostRatingAsync(new Show() { Id = showId }, score, new AnimuluUser() { Id = userId });
            var actualCount = await service.GetRatingAmoutAsync(new Show() { Id = showId });
            var acutalScore = (await service.GetRatingAsync(new Show() { Id = showId }, new AnimuluUser() { Id = userId })).Value;
            Assert.Equal(expectedCount, actualCount);
            Assert.Equal(score, acutalScore);
        }
    }
}
