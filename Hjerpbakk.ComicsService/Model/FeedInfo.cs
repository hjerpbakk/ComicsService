using System;

namespace Hjerpbakk.ComicsService.Model
{
    public struct FeedInfo
    {
        public FeedInfo(string name, DateTime? lastPublishDate)
        {
            Name = name;
            if (lastPublishDate == null) {
                Published = "Has never been publish by this service.";
            } else {
                var age = ConfigurableDateTime.UtcNow - lastPublishDate.Value;
                var daysString = age.Days == 0 ? $"{age.Hours} hours ago" : $"{(age.Days == 1 ? "yesterday" : $"{age.Days} days ago")}";
                Published = $"Latest comic {daysString}.";
            }
        }

        public string Name { get; }
        public string Published { get; }
    }
}
