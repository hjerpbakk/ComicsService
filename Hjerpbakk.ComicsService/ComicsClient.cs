using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Hjerpbakk.ComicsService.Model;

namespace Hjerpbakk.ComicsService
{
    public class ComicsClient
    {
        const string LunchFeed = "https://comics.io/lunchdb/feed/?key=3f0426c84d9041848005f9cb375dbbe2";

        public async Task<string> GetNewestComicAsync() {
            var feed = await FeedReader.ReadAsync(LunchFeed);
            var firstItem = feed.Items.First();
            var lunchItem = new LunchFeedItem(firstItem.Description);
            return lunchItem.ImageURL;
        }
    }
}
