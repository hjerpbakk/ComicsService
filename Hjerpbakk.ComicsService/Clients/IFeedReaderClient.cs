using System.Threading.Tasks;
using CodeHollow.FeedReader;

namespace Hjerpbakk.ComicsService.Clients
{
    public interface IFeedReaderClient
    {
        Task<Feed> ReadAsync(string feedURL);
    }
}
