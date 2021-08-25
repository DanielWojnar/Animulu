using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Data;
using Animulu.Models;
using Microsoft.EntityFrameworkCore;
namespace Animulu.Services.Implementations
{
    public class NavPageService : INavPageService
    {
        private readonly AnimuluContext _context;
        private readonly IdentityContext _icontext;
        public NavPageService(AnimuluContext context, IdentityContext icontext)
        {
            _context = context;
            _icontext = icontext;
        }

        public async Task<NavPage> GetPopularAsync(int pageid)
        {
            var count = 0;
            try
            {
                count = await (from s in _context.Shows select s).AsNoTracking().CountAsync();
            }
            catch
            {
                count = 0;
            }
            return setValues(pageid, count);
        }
        public async Task<NavPage> GetUserRolesAsync(int pageid)
        {
            var count = 0;
            try
            {
                count = await (from s in _icontext.UserRoles select s).AsNoTracking().CountAsync();
            }
            catch
            {
                count = 0;
            }
            return setValues(pageid, count);
        }
        public async Task<NavPage> GetEpisodesAsync(int pageid, int perpage, int showId = 0)
        {
            int count;
            try
            {
                var query = from e in _context.Episodes
                            select e;
                if (showId != 0)
                {
                    query = query.Where(e => e.ShowId == showId);
                }
                count = await query.AsNoTracking().CountAsync();
                
            }
            catch
            {
                count = 0;
            }
            return setValues(pageid, count, perpage);
        }
        public async Task<NavPage> GetShowsAsync(int pageid, int perpage, string title = null)
        {
            int count;
            try
            {
                var query = from s in _context.Shows
                            select s;
                if (title != null)
                {
                    query = query.Where(s => s.Title.Contains(title));
                }
                count = await query.AsNoTracking().CountAsync();
            }
            catch
            {
                count = 0;
            }
            return setValues(pageid, count, perpage);
        }
        public async Task<NavPage> GetSearchAsync(string quote, string tag, string order,int pageid)
        {
            int count = 0;
            var query = from s in _context.Shows
                        select s;
            if (quote != "" && quote != null)
            {
                query = query.Where(s => s.Title.Contains(quote));
            }
            if (tag != "" && tag != null)
            {

                var tags = tag.Split(' ');
                var tagsQ = from tc in _context.TagConnections
                            join ta in _context.Tags on tc.TagId equals ta.Id
                            where tags.Any(t => ta.Name.Replace(" ", "-") == t)
                            group tc.ShowId by tc.ShowId into g
                            select new { ShowId = g.Key, Count = g.Count() };
                query = from s in query
                        join t in tagsQ on s.Id equals t.ShowId
                        where t.Count == tags.Count()
                        select s;
            }
            switch (order)
            {
                case "rdate":
                    query.OrderByDescending(s => s.ReleaseDate);
                    break;
                case "mviews":
                    var mostv = (from v in _context.Views
                                 group v.ShowId by v.ShowId into g
                                 select new { ShowId = (int?)g.Key, Views = (int?)g.Count() });
                    query = from s in query
                            join t in mostv on s.Id equals t.ShowId into ShowGroup
                            from sg in ShowGroup.DefaultIfEmpty()
                            orderby sg.Views descending
                            select s;
                    break;
                case "popular":
                    var trending = (from v in _context.Views
                                    where v.ViewDate >= DateTime.Now.AddDays(-31)
                                    group v.ShowId by v.ShowId into g
                                    select new { ShowId = (int?)g.Key, Views = (int?)g.Count() });
                    query = from s in query
                            join t in trending on s.Id equals t.ShowId into ShowGroup
                            from sg in ShowGroup.DefaultIfEmpty()
                            orderby sg.Views descending
                            select s;
                    break;
                case "rating":
                    break;
                case "alph":
                    query.OrderByDescending(s => s.Title);
                    break;
                default:
                    var deford = (from v in _context.Views
                                  where v.ViewDate >= DateTime.Now.AddDays(-31)
                                  group v.ShowId by v.ShowId into g
                                  select new { ShowId = (int?)g.Key, Views = (int?)g.Count() });
                    query = from s in query
                            join t in deford on s.Id equals t.ShowId into ShowGroup
                            from sg in ShowGroup.DefaultIfEmpty()
                            orderby sg.Views descending
                            select s;
                    break;
            }
            try
            {
                count = await query.AsNoTracking().CountAsync();
            }
            catch
            {
                count = 0;
            }
            return setValues(pageid, count);
        }
        private NavPage setValues(int pageid, int count, int perpage = 25)
        {
            var result = new NavPage();
            if (count % perpage != 0)
            {
                result.TotPages = (count / perpage) + 1;
            }
            else
            {
                result.TotPages = count / perpage;
            }
            if (pageid < 1)
            {
                pageid = 1;
            }
            else if (pageid > result.TotPages)
            {
                pageid = result.TotPages;
            }
            if (pageid - 2 < 1)
            {
                result.MinPage = 1;
            }
            else
            {
                result.MinPage = pageid - 2;
            }
            if (result.MinPage + 7 > result.TotPages)
            {
                result.MaxPage = result.TotPages - result.MinPage + 1;
            }
            else
            {
                result.MaxPage = 8;
            }
            result.CurPage = pageid;
            return result;
        }
    }
}
