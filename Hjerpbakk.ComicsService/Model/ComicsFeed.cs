namespace Hjerpbakk.ComicsService.Model
{
    public struct ComicsFeed
    {
        public ComicsFeed(string name)
        {
            Name = name;
            URL = "https://comics.io/" + name + "/feed/?key=" + ComicsioKey;
        }

        public static string ComicsioKey { private get; set; }

        public string Name { get; }
        public string URL { get;  }

        public override string ToString() => string.Format("[ComicsFeed: Name={0}, URL={1}]", Name, URL);
    }
}
