using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Animulu.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Animulu.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Animulu.Models;
using Animulu.Services.Implementations;

namespace Animulu
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddTransient<IShowService, ShowService>();
            services.AddTransient<IEpisodeService, EpisodeService>();
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<INavPageService, NavPageService>();
            services.AddTransient<IRatingService, RatingService>();
            services.AddTransient<IViewsService, ViewsService>();
            services.AddTransient<ILikeService, LikeService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddHttpContextAccessor();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddRazorPages();
            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/index", "home");
                options.Conventions.AddAreaPageRoute("Administration", "/index", "administration/shows/{pageid=1}");
            });
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                                  policy.RequireRole("Admin"));
            });
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromSeconds(0);
            });
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddServerSideBlazor();
            services.AddHttpClient();
            var connection = Configuration.GetConnectionString("animulu_db");
            services.AddDbContext<AnimuluContext>(options => options.UseSqlServer(connection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".mpd"] = "application/dash+xml";
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
            });
        }
    }
}
