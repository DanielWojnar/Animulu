using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animulu.Models;
using Animulu.Data;
using Microsoft.EntityFrameworkCore;


namespace Animulu.Services.Implementations
{

    public class ShowService : IShowService
    {

        private readonly AnimuluContext _context;
        public ShowService(AnimuluContext context)
        {
            _context = context;
        }
        public async Task RemoveShowAsync(int id)
        {
            var show = new Show { Id = id };
            _context.Shows.Attach(show);
            _context.Shows.Remove(show);
            await _context.SaveChangesAsync();
        }
        public async Task PostShowAsync(Show show)
        {
            await _context.Shows.AddAsync(show);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Show>> GetShowsAsync(string queryTitle = null, int skip = 0, int take = 0)
        {
            try
            {
                var query = from s in _context.Shows
                            select s;
                if(queryTitle != null)
                {
                    query = query.Where(s => s.Title.Contains(queryTitle));
                }
                if(skip != 0)
                {
                    query = query.Skip(skip);
                }
                if(take != 0)
                {
                    query = query.Take(take);
                }
                var result = await query.AsNoTracking().ToListAsync();
                return result;
            }
            catch
            {
                return new List<Show>();
            }
        }
        public async Task<IEnumerable<ShowDisplay>> GetPopularAsync(int skip, int take)
        {
            try
            {
                var rating = from r in _context.Reviews
                             group r by r.ShowId into g
                             select new { ShowId = (int?)g.Key, Count = (float?)g.Count(), Sum = (float?)g.Sum(s => s.Value) };
                var trending = from v in _context.Views
                               where v.ViewDate >= DateTime.Now.AddDays(-31)
                               group v by v.ShowId into g
                               select new { ShowId = (int?)g.Key, Views = (int?)g.Count() };
                var result = await (from s in _context.Shows
                                    join r in rating on s.Id equals r.ShowId into ShowGroup1
                                    from sg1 in ShowGroup1.DefaultIfEmpty()
                                    join t in trending on s.Id equals t.ShowId into ShowGroup2
                                    from sg2 in ShowGroup2.DefaultIfEmpty()
                                    orderby sg2.Views descending
                                    select new ShowDisplay
                                    {
                                        Id = s.Id,
                                        Title = s.Title,
                                        Description = s.Description,
                                        CoverImg = s.CoverImg,
                                        ReleaseDate = s.ReleaseDate,
                                        Score = sg1.Count == null ? 0 : (float)(sg1.Sum / sg1.Count)
                                    }).Skip(skip).Take(take).AsNoTracking().ToListAsync();
                return result;
            }
            catch
            {
                return new List<ShowDisplay>();
            }

        }
        public async Task<IEnumerable<ShowDisplay>> GetRandomAsync(int skip, int take)
        {
            try
            {
                var rating = from r in _context.Reviews
                             group r by r.ShowId into g
                             select new { ShowId = (int?)g.Key, Count = (float?)g.Count(), Sum = (float?)g.Sum(s => s.Value) };
                var result = await (from s in _context.Shows
                                    join r in rating on s.Id equals r.ShowId into ShowGroup
                                    from sg in ShowGroup.DefaultIfEmpty()
                                    select new ShowDisplay
                                    {
                                        Id = s.Id,
                                        Title = s.Title,
                                        Description = s.Description,
                                        CoverImg = s.CoverImg,
                                        ReleaseDate = s.ReleaseDate,
                                        Score = sg.Count == null ? 0 : (float)(sg.Sum / sg.Count)
                                    }).OrderBy(s => Guid.NewGuid()).Skip(skip).Take(take).AsNoTracking().ToListAsync();
                return result;
            }
            catch
            {
                return new List<ShowDisplay>();
            }
        }
        public async Task<Show> GetFeaturedAsync()
        {
            try
            {
                //put here your featured show algorthm
                var result = await (from s in _context.Shows
                                    where s.Id == 22
                                    select s).AsNoTracking().FirstAsync();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public async Task<Show> GetShowAsync(int id)
        {
            try
            {
                var result = await (from s in _context.Shows
                                    where s.Id == id
                                    select s).AsNoTracking().FirstAsync();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public async Task<Show> GetShowAsync(string name)
        {
            try
            {
                var result = await (from s in _context.Shows
                                    where s.Title.Replace(" ", "-") == name
                                    select s).AsNoTracking().FirstAsync();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public async Task<IEnumerable<ShowDisplay>> GetSearchAsync(int skip, int take, string quote, string tag, string order)
        {
            var rating = from r in _context.Reviews
                         group r by r.ShowId into g
                         select new { ShowId = (int?)g.Key, Score = (float?)((float)g.Sum(s => s.Value)/(float)g.Count()) };

            var query = from s in _context.Shows
                        join r in rating on s.Id equals r.ShowId into ShowGroup
                        from sg in ShowGroup.DefaultIfEmpty()
                        orderby sg.Score descending
                        select new ShowDisplay
                        {
                            Id = s.Id,
                            Title = s.Title,
                            Description = s.Description,
                            CoverImg = s.CoverImg,
                            ReleaseDate = s.ReleaseDate,
                            Score = sg.Score == null ? 0 : (float)sg.Score
                        };
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
                    query = query.OrderByDescending(s => s.ReleaseDate);
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
                    query = query.OrderBy(s => s.Title);
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
                var result = await query.Skip(skip).Take(take).AsNoTracking().ToListAsync();
                return result;
            }
            catch
            {
                var result = new List<ShowDisplay>();
                return result;
            }
        }
    }
}
