using Microsoft.Extensions.Caching.Memory;

namespace Hjerpbakk.ComicsService.Cache
{
    public interface ICache
    {
        (bool hit, TItem value) TryGetValue<TItem>(string key);
        void Set<TItem>(string key, TItem value, MemoryCacheEntryOptions options);
    }
}
