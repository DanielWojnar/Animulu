using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Animulu.Data;
using Animulu.Models;
[assembly: HostingStartup(typeof(Animulu.Areas.Identity.IdentityHostingStartup))]
namespace Animulu.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("animuluUser_db")));
                services.AddDefaultIdentity<AnimuluUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<IdentityContext>();
                services.ConfigureApplicationCookie(options => {
                    options.AccessDeniedPath = "/accessdenied";
                    options.LoginPath = "/signin"; 
                });
            });
        }
    }
}
