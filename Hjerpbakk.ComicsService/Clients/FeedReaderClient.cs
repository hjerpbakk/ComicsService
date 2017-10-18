using System.Threading.Tasks;
using CodeHollow.FeedReader;

namespace Hjerpbakk.ComicsService.Clients
{
    public class FeedReaderClient : IFeedReaderClient
    {
        public async Task<Feed> ReadAsync(string feedURL) =>
            await FeedReader.ReadAsync(feedURL);
    }
}
