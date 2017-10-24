using System;
using System.Linq;
using System.Threading.Tasks;
using Hjerpbakk.ComicsService.Cache;
using Hjerpbakk.ComicsService.Configuration;
using Hjerpbakk.ComicsService.Model;
using Microsoft.Extensions.Caching.Memory;

namespace Hjerpbakk.ComicsService.Clients
{
    public class ComicsClient
    {
        const double DayCutoff = 4D;

        static readonly object lockObject = new object();

        readonly ICache memoryCache;
        readonly IFeedReaderClient feedReaderClient;
        readonly ComicsFeed[] feeds;
        readonly Random random;

        int i;

        public ComicsClient(IReadOnlyAppConfiguration configuration, ICache memoryCache, IFeedReaderClient feedReaderClient)
        {
            ComicsFeed.ComicsioKey= configuration.ComicsioKey;
            this.memoryCache = memoryCache;
            this.feedReaderClient = feedReaderClient;

            // TODO: Make configurable
            feeds = new[] {
                new ComicsFeed("oatmeal"),
                new ComicsFeed("lunchdb"),
                new ComicsFeed("cyanideandhappiness"),
                new ComicsFeed("xkcd"),
                new ComicsFeed("commitstrip"),
                new ComicsFeed("redmeat"),
                new ComicsFeed("rocky"),
                new ComicsFeed("smbc"),
                new ComicsFeed("abstrusegoose")
            };

            random = new Random();
		}

        public async Task<string> GetLatestComicAsync() {
            var today = ConfigurableDateTime.UtcNow.Date;
            ComicsItem comic;
            var breakCounter = 0;
            do
            {
				comic = await GetLatestComicFromFeedAsync(feeds[i]);
				lock (lockObject)
				{
                    ++breakCounter;
					if (++i == feeds.Length)
					{
						i = 0;
					}
				}
            } while (comic.PublicationDate + TimeSpan.FromDays(DayCutoff) < today && breakCounter < feeds.Length);
            return comic.ImageURL;
        }

        // TODO: Only fetch new comics
        public async Task<string> GetLatestComicFromRandomFeedAsync() {
            var feedIndex = random.Next(feeds.Length);
            return (await GetLatestComicFromFeedAsync(feeds[feedIndex])).ImageURL;
        }

		async Task<ComicsItem> GetLatestComicFromFeedAsync(ComicsFeed comicsFeed)
		{
            var cachedItem = memoryCache.TryGetValue<ComicsItem>(comicsFeed.Name);
            if (cachedItem.hit) 
			{
                return cachedItem.value;
			}

            var feed = await feedReaderClient.ReadAsync(comicsFeed.URL);
            var firstItem = feed.Items.FirstOrDefault();
            if (firstItem == null) {
                return new ComicsItem();
            }
            var comic = new ComicsItem(firstItem);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(6));

            memoryCache.Set(comicsFeed.Name, comic, cacheEntryOptions);
			return comic;
		}
    }
}
