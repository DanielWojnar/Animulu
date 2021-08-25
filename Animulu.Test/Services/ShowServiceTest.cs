using System;
using Xunit;
using Animulu.Services.Implementations;
using Animulu.Models;
using Animulu.Data;

namespace Animulu.Test.Services
{
    public class ShowServiceTest : AnimuluContextTestBase
    {
        private readonly Show[] _shows;
        public ShowServiceTest()
        {
            _shows = AnimuluContextTestInitializer.shows;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetShowAsync_TakesId_ShouldWork(int index)
        {
            var expected = _shows[index];

            var service = new ShowService(_context);
            var actual = await service.GetShowAsync(expected.Id);

            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.CoverImg, actual.CoverImg);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.ReleaseDate, actual.ReleaseDate);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(200)]
        [InlineData(360)]
        public async void GetShowAsync_TakesId_ShouldReturnNull(int id)
        {
            var service = new ShowService(_context);
            var actual = await service.GetShowAsync(id);

            Assert.Null(actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetShowAsync_TakesName_ShouldWork(int index)
        {
            var expected = _shows[index];
            var name = expected.Title.Replace(" ", "-");

            var service = new ShowService(_context);
            var actual = await service.GetShowAsync(name);

            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.CoverImg, actual.CoverImg);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.ReleaseDate, actual.ReleaseDate);
        }

        [Theory]
        [InlineData("Not existing")]
        [InlineData("There is no show like this")]
        [InlineData("Should return null")]
        public async void GetShowAsync_TakesName_ShouldReturnNull(string name)
        {
            var service = new ShowService(_context);
            var actual = await service.GetShowAsync(name);

            Assert.Null(actual);
        }

        [Theory]
        [InlineData("New show", "new show description", "imagee.png", "01-01-2021")]
        [InlineData("Secound new show", "also a show description", "imagine.png", "04-02-2019")]
        [InlineData("Another new show", "again a show description", "coverr.png", "06-05-2010")]
        public async void PostShowAsync_ShouldWork(string title, string description, string coverImg, string releaseDate)
        {
            var expected = new Show { Title = title, Description = description, CoverImg = coverImg, ReleaseDate = DateTime.Parse(releaseDate) };
            var count = _shows.Length;

            var service = new ShowService(_context);
            await service.PostShowAsync(expected);
            var actual = await service.GetShowAsync(count + 1);

            Assert.NotNull(actual);
            Assert.Equal(count+1, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.CoverImg, actual.CoverImg);
            Assert.Equal(expected.ReleaseDate, actual.ReleaseDate);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void RemoveShowAsync_ShouldWork(int id)
        {
            var expectedCount = _shows.Length - 1;

            var service = new ShowService(_context);
            await service.RemoveShowAsync(id);
            var actualCount = (await service.GetShowsAsync()).Count;
            var show = await service.GetShowAsync(id);

            Assert.Null(show);
            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData(null, 0, 0)]
        [InlineData("Some", 0, 0, 2)]
        [InlineData(null, 1, 0)]
        [InlineData(null, 0, 2)]
        [InlineData("Some", 1, 1)]
        [InlineData("This will never be in any title", 0, 0, 0)]
        public async void GetShows_ShouldWork(string query, int skip, int take, int? expectedCount = null)
        {
            if(expectedCount == null)
            {
                if (take != 0)
                {
                    expectedCount = take;
                }
                else
                {
                    expectedCount = _shows.Length - skip;
                }
            }
            var service = new ShowService(_context);
            var actuals = await service.GetShowsAsync(query, skip, take);

            Assert.Equal(expectedCount, actuals.Count);
            if(query != null)
            {
                foreach(var actual in actuals)
                {
                    Assert.Contains(query, actual.Title);
                }
            }
        }

        [Theory]
        [InlineData(0,3)]
        [InlineData(1,2)]
        [InlineData(0,2)]
        public async void GetPopular_ShouldWork(int skip, int take)
        {
            var expectedCount = take;
            var service = new ShowService(_context);
            var actuals = await service.GetPopularAsync(skip, take);
            var actualCount = skip;
            foreach(var actual in actuals)
            {
                actualCount++;
                Assert.Equal("Popular Show nr " + actualCount, actual.Title);
            }
            Assert.Equal(expectedCount, actualCount - skip);
        }

        [Theory]
        [InlineData(0, 3)]
        [InlineData(1, 2)]
        [InlineData(0, 2)]
        public async void GetRandom_ShouldWork(int skip, int take)
        {
            var expectedCount = take;
            var service = new ShowService(_context);
            var actuals = await service.GetRandomAsync(skip, take);
            var actualCount = skip;
            foreach (var actual in actuals)
            {
                actualCount++;
            }
            Assert.Equal(expectedCount, actualCount - skip);
        }

        [Fact]
        public async void GetSearch_Alph_ShouldWork()
        {
            int skip = 0; int take = 3; string quote = ""; string tag = ""; string order = "alph";
            var expectedTitiles = new string[] { "Aaa", "Bbb", "Ddd" };
            var expectedCount = take;
            var service = new ShowService(_context);
            var actuals = await service.GetSearchAsync(skip, take, quote, tag, order);
            var actualCount = skip;
            var i = 0;
            foreach (var actual in actuals)
            {
                Assert.Equal(expectedTitiles[i], actual.Title);
                actualCount++;
                i++;
            }
            Assert.Equal(expectedCount, actualCount - skip);
        }

        [Fact]
        public async void GetSearch_rdate_ShouldWork()
        {
            int skip = 0; int take = 3; string quote = ""; string tag = ""; string order = "rdate";
            var expectedTitiles = new string[] { "Ddd", "Aaa", "Bbb" };
            var expectedCount = take;
            var service = new ShowService(_context);
            var actuals = await service.GetSearchAsync(skip, take, quote, tag, order);
            var actualCount = skip;
            var i = 0;
            foreach (var actual in actuals)
            {
                Assert.Equal(expectedTitiles[i], actual.Title);
                actualCount++;
                i++;
            }
            Assert.Equal(expectedCount, actualCount - skip);
        }

        [Fact]
        public async void GetSearch_popular_ShouldWork()
        {
            int skip = 0; int take = 3; string quote = ""; string tag = ""; string order = "popular";
            var expectedTitiles = new string[] { "Popular Show nr 1", "Popular Show nr 2", "Popular Show nr 3" };
            var expectedCount = take;
            var service = new ShowService(_context);
            var actuals = await service.GetSearchAsync(skip, take, quote, tag, order);
            var actualCount = skip;
            var i = 0;
            foreach (var actual in actuals)
            {
                Assert.Equal(expectedTitiles[i], actual.Title);
                actualCount++;
                i++;
            }
            Assert.Equal(expectedCount, actualCount - skip);
        }

        [Fact]
        public async void GetSearch_rating_ShouldWork()
        {
            int skip = 0; int take = 2; string quote = ""; string tag = ""; string order = "rating";
            var expectedTitiles = new string[] { "Some title again", "Popular Show nr 1" };
            var expectedCount = take;
            var service = new ShowService(_context);
            var actuals = await service.GetSearchAsync(skip, take, quote, tag, order);
            var actualCount = skip;
            var i = 0;
            foreach (var actual in actuals)
            {
                Assert.Equal(expectedTitiles[i], actual.Title);
                actualCount++;
                i++;
            }
            Assert.Equal(expectedCount, actualCount - skip);
        }

        [Fact]
        public async void GetSearch_mviews_ShouldWork()
        {
            int skip = 0; int take = 3; string quote = ""; string tag = ""; string order = "mviews";
            var expectedTitiles = new string[] { "Test title", "Popular Show nr 1", "Popular Show nr 2"};
            var expectedCount = take;
            var service = new ShowService(_context);
            var actuals = await service.GetSearchAsync(skip, take, quote, tag, order);
            var actualCount = skip;
            var i = 0;
            foreach (var actual in actuals)
            {
                Assert.Equal(expectedTitiles[i], actual.Title);
                actualCount++;
                i++;
            }
            Assert.Equal(expectedCount, actualCount - skip);
        }

        [Fact]
        public async void GetSearch_query_ShouldWork()
        {
            int skip = 0; int take = 5; string quote = "Some"; string tag = ""; string order = "";
            var expectedCount = 2;
            var service = new ShowService(_context);
            var actuals = await service.GetSearchAsync(skip, take, quote, tag, order);
            var actualCount = skip;
            foreach (var actual in actuals)
            {
                actualCount++;
            }
            Assert.Equal(expectedCount, actualCount - skip);
        }

        [Fact]
        public async void GetSearch_tag_ShouldWork1()
        {
            int skip = 0; int take = 5; string quote = ""; string tag = "Cool"; string order = "";
            var expectedCount = 2;
            var service = new ShowService(_context);
            var actuals = await service.GetSearchAsync(skip, take, quote, tag, order);
            var actualCount = skip;
            foreach (var actual in actuals)
            {
                actualCount++;
            }
            Assert.Equal(expectedCount, actualCount - skip);
        }

        [Fact]
        public async void GetSearch_tag_ShouldWork2()
        {
            int skip = 0; int take = 5; string quote = ""; string tag = "Cool Test-Tag Amazing"; string order = "";
            var expectedCount = 1;
            var service = new ShowService(_context);
            var actuals = await service.GetSearchAsync(skip, take, quote, tag, order);
            var actualCount = skip;
            foreach (var actual in actuals)
            {
                actualCount++;
            }
            Assert.Equal(expectedCount, actualCount - skip);
        }
    }
}
