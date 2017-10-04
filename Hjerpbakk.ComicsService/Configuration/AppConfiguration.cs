using System;
namespace Hjerpbakk.ComicsService.Configuration
{
    public class AppConfiguration : IReadOnlyAppConfiguration
    {
        public string ComicsioKey { get; set; }
    }
}
