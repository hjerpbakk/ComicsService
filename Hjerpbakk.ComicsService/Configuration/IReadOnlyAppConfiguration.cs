using System;
namespace Hjerpbakk.ComicsService.Configuration
{
    public interface IReadOnlyAppConfiguration
    {
        string ComicsioKey { get; }
    }
}
