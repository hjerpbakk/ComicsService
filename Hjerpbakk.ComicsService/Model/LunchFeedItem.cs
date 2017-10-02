using System;
using System.Text.RegularExpressions;

namespace Hjerpbakk.ComicsService.Model
{
    public struct LunchFeedItem
    {
        static readonly Regex imageURLRegex;

        static LunchFeedItem() {
            imageURLRegex = new Regex("(src=\\\")(.*?)(\\\")", RegexOptions.Compiled);
        }

        public LunchFeedItem(string content)
        {
            var matches = imageURLRegex.Matches(content);
            if (matches[0].Groups.Count != 4) {
                throw new Exception($"Could not parse {content}");
            }

            ImageURL = matches[0].Groups[2].Value;
        }

        public string ImageURL { get; }
    }
}
