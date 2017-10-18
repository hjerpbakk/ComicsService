using System;
using System.Text.RegularExpressions;
using CodeHollow.FeedReader;

namespace Hjerpbakk.ComicsService.Model
{
    public struct ComicsItem
    {
        static readonly Regex imageURLRegex;

        static ComicsItem() {
            imageURLRegex = new Regex("(src=\\\")(.*?)(\\\")", RegexOptions.Compiled);
        }

        public ComicsItem(FeedItem feedItem)
        {
            var matches = imageURLRegex.Matches(feedItem.Description);
            if (matches.Count == 0 || matches[0].Groups.Count != 4) {
                throw new Exception($"Could not parse {feedItem.Description}");
            }

            ImageURL = matches[0].Groups[2].Value;

            // /oatmeal/2017/9/22/
            if (feedItem.PublishingDate.HasValue) {
                PublicationDate = feedItem.PublishingDate.Value.Date;
                return;
            }

            var split = feedItem.Id.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (split.Length == 4) {
                PublicationDate = new DateTime(int.Parse(split[1]), int.Parse(split[2]), int.Parse(split[3]));
                return;
            }

            PublicationDate = ConfigurableDateTime.UtcNow.Date;
        }

        public string ImageURL { get; }
        public DateTime PublicationDate { get; }

        public override string ToString() => string.Format("[ComicsItem: ImageURL={0}]", ImageURL);
    }
}
