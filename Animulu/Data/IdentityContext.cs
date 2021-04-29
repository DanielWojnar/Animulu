using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Animulu.Models;

namespace Animulu.Data
{
    public class IdentityContext : IdentityDbContext<AnimuluUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
