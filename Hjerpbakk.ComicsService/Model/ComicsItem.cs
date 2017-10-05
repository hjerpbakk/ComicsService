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
            if (matches[0].Groups.Count != 4) {
                throw new Exception($"Could not parse {feedItem.Description}");
            }

            ImageURL = matches[0].Groups[2].Value;
            PublicationDateTime = feedItem.PublishingDate ?? DateTime.UtcNow;
        }

        public string ImageURL { get; }
        public DateTime PublicationDateTime { get; }

        public override string ToString() => string.Format("[ComicsItem: ImageURL={0}]", ImageURL);
    }
}
