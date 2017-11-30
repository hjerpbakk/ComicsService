using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Hjerpbakk.ComicsService;
using Hjerpbakk.ComicsService.Cache;
using Hjerpbakk.ComicsService.Clients;
using Hjerpbakk.ComicsService.Configuration;
using Hjerpbakk.ComicsService.Model;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace TestHjerpbakk.ComicsService
{
    public class ComicsClientTests
    {
        [Fact]
        public async Task GetLatestComicAsync_OnlyComicsFromUpdatedFeedsAreReturned()
        {
            var today = new DateTime(2017, 10, 9);
            ConfigurableDateTime.CurrentTime = today;
            var feeds = new[] {
                new Feed {
                    Items = new List<FeedItem> {
                        new FeedItem {
                            Description = "src=\"https://static.comics.io/media/lunchdb/6/62df69ad0363b2221ab30ad0a5d53af00671ff09d4f754d4d92bfabb483bef44.jpg\"",
                            PublishingDate = today
                        }
                    }
                },
                new Feed
                {
                    Items = new List<FeedItem> {
                        new FeedItem {
                            Description = "src=\"https://static.comics.io/media/cyanideandhappiness/6/62df69ad0363b2221ab30ad0a5d53af00671ff09d4f754d4d92bfabb483bef44.jpg\"",
                            PublishingDate = today - TimeSpan.FromDays(1D)
                        }
                    }
                },
                    new Feed
                {
                    Items = new List<FeedItem> {
                        new FeedItem {
                            Description = "src=\"https://static.comics.io/media/xkcd/6/62df69ad0363b2221ab30ad0a5d53af00671ff09d4f754d4d92bfabb483bef44.jpg\"",
                            PublishingDate = today - TimeSpan.FromDays(2D)
                        }
                    }
                },
                    new Feed
                {
                    Items = new List<FeedItem> {
                        new FeedItem {
                            Description = "src=\"https://static.comics.io/media/commitstrip/6/62df69ad0363b2221ab30ad0a5d53af00671ff09d4f754d4d92bfabb483bef44.jpg\"",
                            PublishingDate = today - TimeSpan.FromDays(3D)
                        }
                    }
                },
                    new Feed
                {
                    Items = new List<FeedItem> {
                        new FeedItem {
                            Description = "src=\"https://static.comics.io/media/redmeat/6/62df69ad0363b2221ab30ad0a5d53af00671ff09d4f754d4d92bfabb483bef44.jpg\"",
                            PublishingDate = today - TimeSpan.FromDays(4D)
                        }
                    }
                },
                    new Feed
                {
                    Items = new List<FeedItem> {
                        new FeedItem {
                            Description = "src=\"https://static.comics.io/media/rocky/6/62df69ad0363b2221ab30ad0a5d53af00671ff09d4f754d4d92bfabb483bef44.jpg\"",
                            PublishingDate = today - TimeSpan.FromDays(5D)
                        }
                    }
                },
                    new Feed
                {
                    Items = new List<FeedItem> {
                        new FeedItem {
                            Description = "src=\"https://static.comics.io/media/smbc/6/62df69ad0363b2221ab30ad0a5d53af00671ff09d4f754d4d92bfabb483bef44.jpg\"",
                            PublishingDate = today - TimeSpan.FromDays(6D)
                        }
                    }
                }
            };

            var items = CreateClient();
            items.feedReaderClient.Setup(f => f.ReadAsync(It.IsRegex("lunchdb"))).ReturnsAsync(feeds[0]);
            items.feedReaderClient.Setup(f => f.ReadAsync(It.IsRegex("cyanideandhappiness"))).ReturnsAsync(feeds[1]);
            items.feedReaderClient.Setup(f => f.ReadAsync(It.IsRegex("xkcd"))).ReturnsAsync(feeds[2]);
            items.feedReaderClient.Setup(f => f.ReadAsync(It.IsRegex("commitstrip"))).ReturnsAsync(feeds[3]);
            items.feedReaderClient.Setup(f => f.ReadAsync(It.IsRegex("redmeat"))).ReturnsAsync(feeds[4]);
            items.feedReaderClient.Setup(f => f.ReadAsync(It.IsRegex("rocky"))).ReturnsAsync(feeds[5]);
            items.feedReaderClient.Setup(f => f.ReadAsync(It.IsRegex("smbc"))).ReturnsAsync(feeds[6]);
            var comicsClient = items.comicsClient;

            var comic = await comicsClient.GetLatestComicFromFeedsRoundRobinAsync();
            Assert.Contains("lunchdb", comic);

			comic = await comicsClient.GetLatestComicFromFeedsRoundRobinAsync();
			Assert.Contains("cyanideandhappiness", comic);

			comic = await comicsClient.GetLatestComicFromFeedsRoundRobinAsync();
			Assert.Contains("xkcd", comic);

			comic = await comicsClient.GetLatestComicFromFeedsRoundRobinAsync();
			Assert.Contains("commitstrip", comic);

			comic = await comicsClient.GetLatestComicFromFeedsRoundRobinAsync();
			Assert.Contains("lunchdb", comic);

			comic = await comicsClient.GetLatestComicFromFeedsRoundRobinAsync();
			Assert.Contains("cyanideandhappiness", comic);

			comic = await comicsClient.GetLatestComicFromFeedsRoundRobinAsync();
			Assert.Contains("xkcd", comic);
        }

        (ComicsClient comicsClient, Mock<IFeedReaderClient> feedReaderClient) CreateClient(bool cleanCache = true)
        {
            var memoryCache = new Mock<ICache>();
            var config = new Mock<IReadOnlyAppConfiguration>();
            config.SetupGet(c => c.ComicsioKey).Returns("");
            if (cleanCache)
            {
                memoryCache.Setup(m => m.TryGetValue<ComicsItem>(It.IsAny<string>())).Returns((false, new ComicsItem()));
            }

            var feedReaderClient = new Mock<IFeedReaderClient>();
            return (new ComicsClient(config.Object, memoryCache.Object, feedReaderClient.Object), feedReaderClient);
        }
    }
}
