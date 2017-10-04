using System;
using System.Linq;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Hjerpbakk.ComicsService.Configuration;
using Hjerpbakk.ComicsService.Model;
using Microsoft.Extensions.Caching.Memory;

namespace Hjerpbakk.ComicsService.Clients
{
    public class ComicsClient
    {
        static readonly object lockObject = new object();

        readonly IMemoryCache memoryCache;
        readonly ComicsFeed[] feeds;
        readonly Random random;

        int i;

        public ComicsClient(IReadOnlyAppConfiguration configuration, IMemoryCache memoryCache)
        {
            ComicsFeed.ComicsioKey= configuration.ComicsioKey;
            this.memoryCache = memoryCache;

            feeds = new[] {
                new ComicsFeed("lunchdb"),
                new ComicsFeed("cyanideandhappiness"),
                new ComicsFeed("xkcd"),
                new ComicsFeed("commitstrip"),
                new ComicsFeed("redmeat"),
                new ComicsFeed("rocky")
            };

            random = new Random();
		}

        public async Task<string> GetLatestComicAsync() {
            var imageURL = await GetLatestComicFromFeedAsync(feeds[i]);
            lock (lockObject) {
                if (++i == feeds.Length) {
                    i = 0;
                }
            }

            return imageURL;
        }

        public async Task<string> GetLatestComicFromRandomFeedAsync() {
            var feedIndex = random.Next(feeds.Length);
            return await GetLatestComicFromFeedAsync(feeds[feedIndex]);
        }

        async Task<string> GetLatestComicFromFeedAsync(ComicsFeed comicsFeed)
		{
            if (!memoryCache.TryGetValue(comicsFeed.Name, out string imageURL))
            {
                var feed = await FeedReader.ReadAsync(comicsFeed.URL);
                var firstItem = feed.Items.First();
                var lunchItem = new ComicsItem(firstItem.Description);
                imageURL = lunchItem.ImageURL;

				var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(12));

				memoryCache.Set(comicsFeed.Name, imageURL, cacheEntryOptions);
            }

			return imageURL;
		}
    }
}
