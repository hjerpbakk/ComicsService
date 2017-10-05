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
            var comic = await GetLatestComicFromFeedAsync(feeds[i]);
            lock (lockObject) {
                if (++i == feeds.Length) {
                    i = 0;
                }
            }

            return comic.ImageURL;
        }

        public async Task<string> GetLatestComicFromRandomFeedAsync() {
            var feedIndex = random.Next(feeds.Length);
            return (await GetLatestComicFromFeedAsync(feeds[feedIndex])).ImageURL;
        }

		async Task<ComicsItem> GetLatestComicFromFeedAsync(ComicsFeed comicsFeed)
		{
			if (!memoryCache.TryGetValue(comicsFeed.Name, out ComicsItem comic))
			{
				var feed = await FeedReader.ReadAsync(comicsFeed.URL);
				var firstItem = feed.Items.First();
				comic = new ComicsItem(firstItem);

				var cacheEntryOptions = new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromHours(6));

				memoryCache.Set(comicsFeed.Name, comic, cacheEntryOptions);
			}

			return comic;
		}
    }
}
