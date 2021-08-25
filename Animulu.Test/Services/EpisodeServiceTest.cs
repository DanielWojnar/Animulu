using System;
using Xunit;
using Animulu.Services.Implementations;
using Animulu.Models;
using Animulu.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Animulu.Test.Services
{
    public class EpisodeServiceTest : AnimuluContextTestBase
    {
        private readonly Episode[] _episodes;
        public EpisodeServiceTest()
        {
            _episodes = AnimuluContextTestInitializer.episodes;
        }

        [Theory]
        [InlineData(5, 1, 1)]
        [InlineData(7, 1, 4)]
        [InlineData(1, 1, 5)]
        public async void GetEpisodeAsync_ByShow_ShouldWork(int showId, int index, int expected)
        {
            var service = new EpisodeService(_context);
            var actual = await service.GetEpisodeAsync(new Show() { Id = showId }, index);
            Assert.Equal(expected, actual.Id);
        }

        [Theory]
        [InlineData(3, 30)]
        [InlineData(15, 1)]
        [InlineData(9, 9)]
        public async void GetEpisodeAsync_ByShow_ShouldNotWork(int showId, int index)
        {
            var service = new EpisodeService(_context);
            var actual = await service.GetEpisodeAsync(new Show() { Id = showId }, index);
            Assert.Null(actual);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(2)]
        public async void GetEpisodeAsync_ById_ShouldWork(int expectedId)
        {
            var service = new EpisodeService(_context);
            var actual = await service.GetEpisodeAsync(expectedId);
            Assert.Equal(expectedId, actual.Id);
        }

        [Theory]
        [InlineData(19)]
        [InlineData(15)]
        [InlineData(30)]
        public async void GetEpisodeAsync_ById_ShouldNotWork(int id)
        {
            var service = new EpisodeService(_context);
            var actual = await service.GetEpisodeAsync(id);
            Assert.Null(actual);
        }
        
        [Theory]
        [InlineData(6, 1)]
        [InlineData(5, 2)]
        [InlineData(8, 0)]
        public async void GetEpisodesAsync_ForShow_ShouldWork(int showId, int expected)
        {
            var service = new EpisodeService(_context);
            var actual = await service.GetEpisodesAsync(new Show() { Id = showId });
            var i = 0;
            foreach(var a in actual)
            {
                i++;
            }
            Assert.Equal(expected, i);
        }
        
        [Theory]
        [InlineData(0, 0, 0, 5)]
        [InlineData(5, 0, 0, 2)]
        [InlineData(6, 0, 0, 1)]
        [InlineData(0, 2, 0, 3)]
        [InlineData(0, 0, 2, 2)]
        [InlineData(5, 1, 0, 1)]
        [InlineData(5, 0, 1, 1)]
        [InlineData(5, 6, 1, 0)]
        public async void GetEpisodesAsync_Every_ShouldWork(int showId, int skip, int take, int expected)
        {
            var service = new EpisodeService(_context);
            var actual = await service.GetEpisodesAsync(showId, skip, take);
            var i = 0;
            foreach(var a in actual)
            {
                i++;
            }
            Assert.Equal(expected, i);
        }

        [Theory]
        [InlineData(0, 6, 5)]
        [InlineData(0, 3, 3)]
        [InlineData(2, 3, 3)]
        [InlineData(3, 3, 2)]
        [InlineData(6, 3, 0)]
        public async void GetUploadedAsync_ShouldWork(int skip, int take, int expected)
        {
            var expecteds = new int[5]{ 5, 2, 3, 4, 1 }; 
            var service = new EpisodeService(_context);
            var actual = await service.GetUploadedAsync(skip, take);
            var i = skip;
            foreach (var a in actual)
            {
                Assert.Equal(expecteds[i], a.Id);
                i++;
            }
            Assert.Equal(expected, i - skip);
        }

        [Theory]
        [InlineData(0, 6, 5)]
        [InlineData(0, 3, 3)]
        [InlineData(2, 3, 3)]
        [InlineData(3, 3, 2)]
        [InlineData(6, 3, 0)]
        public async void GetReleasedAsync_ShouldWork(int skip, int take, int expected)
        {
            var expecteds = new int[5] { 3, 2, 5, 1, 4 };
            var service = new EpisodeService(_context);
            var actual = await service.GetReleasedAsync(skip, take);
            var i = skip;
            foreach (var a in actual)
            {
                Assert.Equal(expecteds[i], a.Id);
                i++;
            }
            Assert.Equal(expected, i - skip);
        }

        [Theory]
        [InlineData("New Episode Unique")]
        [InlineData("New Episode Unique 2")]
        [InlineData("New Episode Uniqueee")]
        public async void PostEpisodeAsync_ShouldWork(string title)
        {
            var random = new Random();
            var service = new EpisodeService(_context);
            var expectedCount = (await service.GetEpisodesAsync(0, 0, 0)).Count + 1;
            await service.PostEpisodeAsync(new Episode() { Title = title, Description = "test", EpisodeIndex = random.Next(1,10) , ShowId = random.Next(1, 10), ReleaseDate = DateTime.Now, UploadDate = DateTime.Now, VideoSrc = "test.src" });
            var actualCount = (await service.GetEpisodesAsync(0, 0, 0)).Count;
            var actual = await (from a in _context.Episodes
                                where a.Title == title
                                select a).AsNoTracking().FirstAsync();
            Assert.NotNull(actual);
            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(4)]
        public async void RemoveEpisodeAsync_ShouldWork(int episodeId)
        {
            var service = new EpisodeService(_context);
            var expectedCount = (await service.GetEpisodesAsync(0, 0, 0)).Count - 1;
            await service.RemoveEpisodeAsync(episodeId);
            var actualCount = (await service.GetEpisodesAsync(0, 0, 0)).Count;
            var actual = await (from a in _context.Episodes
                                where a.Id == episodeId
                                select a).AsNoTracking().ToListAsync();
            Assert.Empty(actual);
            Assert.Equal(expectedCount, actualCount);
        }
    }
}
