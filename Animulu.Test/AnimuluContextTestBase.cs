using System;
using System.Collections.Generic;
using System.Text;
using Animulu.Data;
using Animulu.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace Animulu.Test
{
    public class AnimuluContextTestBase : IDisposable
    {
        protected readonly AnimuluContext _context;
        public AnimuluContextTestBase()
        {
            var options = new DbContextOptionsBuilder<AnimuluContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            AnimuluContextTestInitializer.Initialize(options);
            _context = new AnimuluContext(options);
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
