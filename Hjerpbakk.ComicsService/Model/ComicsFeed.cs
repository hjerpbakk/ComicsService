using System;

namespace Hjerpbakk.ComicsService.Model
{
    public class ComicsFeed
    {
        public ComicsFeed(string name)
        {
            Name = name;
            URL = "https://comics.io/" + name + "/feed/?key=" + ComicsioKey;
        }

        public static string ComicsioKey { private get; set; }

        public string Name { get; }
        public string URL { get; }
        public DateTime? LastPublished { get; set; }

        public override string ToString() => string.Format("[ComicsFeed: Name={0}, URL={1}]", Name, URL);
    }
}
