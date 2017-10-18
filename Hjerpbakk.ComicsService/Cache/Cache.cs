using Microsoft.Extensions.Caching.Memory;

namespace Hjerpbakk.ComicsService.Cache
{
    public class Cache : ICache
    {
        readonly IMemoryCache memoryCache;

        public Cache(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public void Set<TItem>(string key, TItem value, MemoryCacheEntryOptions options) =>
            memoryCache.Set(key, value, options);

        public (bool hit, TItem value) TryGetValue<TItem>(string key) =>
            (memoryCache.TryGetValue(key, out TItem value), value);
    }
}
