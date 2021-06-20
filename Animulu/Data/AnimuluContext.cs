using Animulu.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animulu.Data
{
    public class AnimuluContext : DbContext
    {
        public AnimuluContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagConnection> TagConnections { get; set; }
        public DbSet<View> Views { get; set; }
    }
}
