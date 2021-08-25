using System;
using Xunit;
using Animulu.Services.Implementations;
using Animulu.Models;
using Animulu.Data;

namespace Animulu.Test.Services
{
    public class ViewServiceTest : AnimuluContextTestBase
    {
        private readonly View[] _views;
        public ViewServiceTest()
        {
            _views = AnimuluContextTestInitializer.views;
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(2, 0)]
        [InlineData(6, 2)]
        public async void GetViewsAsync_Show_ShouldWork(int id, int expected)
        {
            var service = new ViewsService(_context);
            var actual = await service.GetViewsAsync(new Show() { Id = id });
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(5, 5)]
        [InlineData(2, 2)]
        [InlineData(6, 0)]
        public async void GetViewsAsync_Episode_ShouldWork(int id, int expected)
        {
            var service = new ViewsService(_context);
            var actual = await service.GetViewsAsync(new Episode() { Id = id });
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void PostViewAsync_ShouldWork()
        {
            var service = new ViewsService(_context);
            var expected = await service.GetViewsAsync(new Episode() { Id = 10 }) + 1;
            await service.PostViewAsync(new Show() { Id = 10 }, new Episode() { Id = 10 });
            var actual = await service.GetViewsAsync(new Episode() { Id = 10 });
            Assert.Equal(expected, actual);
        }
    }
}
