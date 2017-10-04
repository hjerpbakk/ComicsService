using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Hjerpbakk.ComicsService.Configuration;
using Hjerpbakk.ComicsService.Model;

namespace Hjerpbakk.ComicsService.Clients
{
    public class ComicsClient
    {
        readonly string LunchFeed = "https://comics.io/lunchdb/feed/?key=";
        readonly string CyanideAndHappiness = "https://comics.io/cyanideandhappiness/feed/?key=";

        readonly string comicsioKey;

        public ComicsClient(IReadOnlyAppConfiguration configuration)
        {
            comicsioKey = configuration.ComicsioKey;
        }

        public async Task<string> GetNewestComicAsync() {
            var feed = await FeedReader.ReadAsync(FeedWithKey(LunchFeed));
            var firstItem = feed.Items.First();
            var lunchItem = new LunchFeedItem(firstItem.Description);
            return lunchItem.ImageURL;
        }

        string FeedWithKey(string feedURL) => feedURL + comicsioKey;
    }
}
