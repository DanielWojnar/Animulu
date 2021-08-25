using Animulu.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Animulu.Models;
using Microsoft.EntityFrameworkCore;

namespace Animulu.Test
{
    public static class AnimuluContextTestInitializer
    {
        public static Show[] shows = {
            new Show { Id = 1, CoverImg = "test.png", Description = "Test description", ReleaseDate = DateTime.Now, Title = "Test title"},
            new Show { Id = 2, CoverImg = "image.png", Description = "Some description", ReleaseDate = DateTime.Now.AddDays(1), Title = "Some title"},
            new Show { Id = 3, CoverImg = "cover.png", Description = "Other description", ReleaseDate = DateTime.Now.AddDays(-1), Title = "Other title"},
            new Show { Id = 4, CoverImg = "coverr.png", Description = "Some description again", ReleaseDate = DateTime.Now.AddDays(-2), Title = "Some title again"},
            new Show { Id = 5, CoverImg = "coverr.png", Description = "Popular Show nr 1 description", ReleaseDate = DateTime.Now.AddDays(1), Title = "Popular Show nr 1"},
            new Show { Id = 6, CoverImg = "coverr.png", Description = "Popular Show nr 2 description", ReleaseDate = DateTime.Now.AddDays(1), Title = "Popular Show nr 2"},
            new Show { Id = 7, CoverImg = "coverr.png", Description = "Popular Show nr 3 description", ReleaseDate = DateTime.Now.AddDays(1), Title = "Popular Show nr 3"},
            new Show { Id = 8, CoverImg = "coverr.png", Description = "aaa description", ReleaseDate = DateTime.Now.AddDays(2), Title = "Aaa"},
            new Show { Id = 9, CoverImg = "coverr.png", Description = "bbb description", ReleaseDate = DateTime.Now.AddDays(1), Title = "Bbb"},
            new Show { Id = 10, CoverImg = "coverr.png", Description = "ccc description", ReleaseDate = DateTime.Now.AddDays(4), Title = "Ddd"}
        };

        public static Episode[] episodes = { 
            new Episode { Id = 1, Description = "Random episode description", EpisodeIndex = 1, ReleaseDate = DateTime.Now.AddDays(-1), ShowId = 5, Title = "Random episode titile", UploadDate = DateTime.Now.AddDays(-2), VideoSrc = "random.mp4" },
            new Episode { Id = 2, Description = "Random episode description", EpisodeIndex = 2, ReleaseDate = DateTime.Now.AddDays(2), ShowId = 5, Title = "Random episode titile", UploadDate = DateTime.Now.AddDays(5), VideoSrc = "random.mp4" },
            new Episode { Id = 3, Description = "Random episode description", EpisodeIndex = 1, ReleaseDate = DateTime.Now.AddDays(8), ShowId = 6, Title = "Random episode titile", UploadDate = DateTime.Now.AddDays(1), VideoSrc = "random.mp4" },
            new Episode { Id = 4, Description = "Random episode description", EpisodeIndex = 1, ReleaseDate = DateTime.Now.AddDays(-2), ShowId = 7, Title = "Random episode titile", UploadDate = DateTime.Now.AddDays(0), VideoSrc = "random.mp4" },
            new Episode { Id = 5, Description = "Random episode description", EpisodeIndex = 1, ReleaseDate = DateTime.Now.AddDays(0), ShowId = 1, Title = "Random episode titile", UploadDate = DateTime.Now.AddDays(12), VideoSrc = "random.mp4" }
        };

        public static Tag[] tags = {
            new Tag { Id = 1, Name = "Test Tag" },
            new Tag { Id = 2, Name = "Cool" },
            new Tag { Id = 3, Name = "Amazing" }
        };

        public static TagConnection[] tagConnections = {
            new TagConnection { Id = 1, ShowId = 1, TagId = 1},
            new TagConnection { Id = 2, ShowId = 1, TagId = 2},
            new TagConnection { Id = 3, ShowId = 2, TagId = 1},
            new TagConnection { Id = 4, ShowId = 2, TagId = 2},
            new TagConnection { Id = 5, ShowId = 2, TagId = 3},
            new TagConnection { Id = 6, ShowId = 3, TagId = 3}
        };

        public static View[] views = {
            new View { Id = 1, EpisodeId = 1, ShowId = 5, ViewDate = DateTime.Now },
            new View { Id = 2, EpisodeId = 1, ShowId = 5, ViewDate = DateTime.Now },
            new View { Id = 3, EpisodeId = 2, ShowId = 5, ViewDate = DateTime.Now },
            new View { Id = 4, EpisodeId = 2, ShowId = 5, ViewDate = DateTime.Now },
            new View { Id = 5, EpisodeId = 3, ShowId = 6, ViewDate = DateTime.Now },
            new View { Id = 6, EpisodeId = 3, ShowId = 6, ViewDate = DateTime.Now },
            new View { Id = 7, EpisodeId = 4, ShowId = 7, ViewDate = DateTime.Now },
            new View { Id = 8, EpisodeId = 5, ShowId = 1, ViewDate = DateTime.Now.AddDays(-40) },
            new View { Id = 9, EpisodeId = 5, ShowId = 1, ViewDate = DateTime.Now.AddDays(-40) },
            new View { Id = 10, EpisodeId = 5, ShowId = 1, ViewDate = DateTime.Now.AddDays(-40) },
            new View { Id = 11, EpisodeId = 5, ShowId = 1, ViewDate = DateTime.Now.AddDays(-40) },
            new View { Id = 12, EpisodeId = 5, ShowId = 1, ViewDate = DateTime.Now.AddDays(-40) }
        };

        public static Review[] reviews = { 
            new Review { Id = 1, ShowId = 4, UserId = "1", Value = 5},
            new Review { Id = 2, ShowId = 4, UserId = "2", Value = 4},
            new Review { Id = 3, ShowId = 4, UserId = "3", Value = 5},
            new Review { Id = 4, ShowId = 4, UserId = "4", Value = 5},
            new Review { Id = 5, ShowId = 5, UserId = "1", Value = 1},
            new Review { Id = 6, ShowId = 5, UserId = "2", Value = 2}
        };

        public static Like[] likes = {
            new Like { Id = 1, EpisodeId = 1, Positive = true, UserId = "1"},
            new Like { Id = 2, EpisodeId = 2, Positive = true, UserId = "4"},
            new Like { Id = 3, EpisodeId = 4, Positive = true, UserId = "3"},
            new Like { Id = 4, EpisodeId = 2, Positive = true, UserId = "3"},
            new Like { Id = 5, EpisodeId = 5, Positive = true, UserId = "3"},
            new Like { Id = 6, EpisodeId = 5, Positive = true, UserId = "5"},
            new Like { Id = 7, EpisodeId = 5, Positive = true, UserId = "6"},
            new Like { Id = 8, EpisodeId = 5, Positive = true, UserId = "abc"},
            new Like { Id = 9, EpisodeId = 8, Positive = false, UserId = "azd"},
            new Like { Id = 10, EpisodeId = 8, Positive = false, UserId = "zxy"},
            new Like { Id = 11, EpisodeId = 2, Positive = false, UserId = "yxz"}
        };

        public static Comment[] comments = {
            new Comment { Id = 1, Content = "Some random comment content", CreationDate = DateTime.Now, EpisodeId = 1, UserId = "1" },
            new Comment { Id = 2, Content = "Some random comment content", CreationDate = DateTime.Now, EpisodeId = 4, UserId = "10" },
            new Comment { Id = 3, Content = "Some random comment content", CreationDate = DateTime.Now, EpisodeId = 9, UserId = "4" },
            new Comment { Id = 4, Content = "Some random comment content", CreationDate = DateTime.Now, EpisodeId = 3, UserId = "3" },
            new Comment { Id = 5, Content = "Some random comment content", CreationDate = DateTime.Now, EpisodeId = 1, UserId = "3" },
            new Comment { Id = 6, Content = "Some random comment content", CreationDate = DateTime.Now, EpisodeId = 5, UserId = "11" },
            new Comment { Id = 7, Content = "Some random comment content", CreationDate = DateTime.Now, EpisodeId = 5, UserId = "12" },
            new Comment { Id = 8, Content = "Some random comment content", CreationDate = DateTime.Now, EpisodeId = 5, UserId = "100" },
            new Comment { Id = 9, Content = "Some random comment content", CreationDate = DateTime.Now, EpisodeId = 5, UserId = "9" },
        };

        public static void Initialize(DbContextOptions options)
        {
            using (var context = new AnimuluContext(options))
            {
                context.Shows.AddRange(shows);
                context.Episodes.AddRange(episodes);
                context.Tags.AddRange(tags);
                context.TagConnections.AddRange(tagConnections);
                context.Views.AddRange(views);
                context.Reviews.AddRange(reviews);
                context.Likes.AddRange(likes);
                context.Comments.AddRange(comments);
                context.SaveChanges();
            }
        }
    }
}
