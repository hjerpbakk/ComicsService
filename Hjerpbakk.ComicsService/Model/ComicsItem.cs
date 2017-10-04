using System;
using System.Text.RegularExpressions;

namespace Hjerpbakk.ComicsService.Model
{
    public struct ComicsItem
    {
        static readonly Regex imageURLRegex;

        static ComicsItem() {
            imageURLRegex = new Regex("(src=\\\")(.*?)(\\\")", RegexOptions.Compiled);
        }

        public ComicsItem(string content)
        {
            var matches = imageURLRegex.Matches(content);
            if (matches[0].Groups.Count != 4) {
                throw new Exception($"Could not parse {content}");
            }

            ImageURL = matches[0].Groups[2].Value;
        }

        public string ImageURL { get; }

        public override string ToString() => string.Format("[ComicsItem: ImageURL={0}]", ImageURL);
    }
}
