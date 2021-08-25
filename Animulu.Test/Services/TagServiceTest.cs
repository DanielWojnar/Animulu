using System;
using Xunit;
using Animulu.Services.Implementations;
using Animulu.Models;
using Animulu.Data;

namespace Animulu.Test.Services
{
    public class TagServiceTest : AnimuluContextTestBase
    {
        private readonly Tag[] _tags;
        public TagServiceTest()
        {
            _tags = AnimuluContextTestInitializer.tags;
        }

        [Fact]
        public async void GetTagsAsync_ShouldWork()
        {
            var expected = new string[] { "Test Tag", "Cool" };
            var service = new TagService(_context);
            var actuals = await service.GetTagsAsync(new Show() { Id = 1 });
            var i = 0;
            foreach(var actual in actuals)
            {
                Assert.Equal(expected[i], actual.Name);
                i++;
            }
        }

        [Fact]
        public async void GetAllNamesAsync_ShouldWork()
        {
            var expected = new string[] { "Amazing", "Cool", "Test Tag" };
            var service = new TagService(_context);
            var actuals = await service.GetAllNamesAsync();
            var i = 0;
            foreach (var actual in actuals)
            {
                Assert.Equal(expected[i], actual);
                i++;
            }
        }
    }
}
