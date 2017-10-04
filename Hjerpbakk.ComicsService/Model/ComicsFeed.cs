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
    }
}
