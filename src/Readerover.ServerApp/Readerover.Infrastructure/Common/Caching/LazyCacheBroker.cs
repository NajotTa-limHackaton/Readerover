using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Readerover.Infrastructure.Common.Settings;
using Readerover.Persistence.Caching;

namespace Readerover.Infrastructure.Common.Caching;

public class LazyCacheBroker(IAppCache appCache, IOptions<CacheSettings> cacheSettings) : ICacheBroker
{
    private readonly CacheSettings _cacheSettings = cacheSettings.Value;

    public ValueTask DeleteAsync(string key)
    {
        appCache.Remove(key);

        return ValueTask.CompletedTask;
    }

    public async ValueTask<T> GetAsync<T>(string key) => await appCache.GetAsync<T>(key);

    public async ValueTask<T> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheSettings.AbsoluteExpirationTimeInSeconds),
            SlidingExpiration = TimeSpan.FromSeconds(_cacheSettings.SlidingExpirationTimeInSeconds)
        };

        return await appCache.GetOrAddAsync(key, valueFactory, options);
    }

    public ValueTask SetAsync<T>(string key, T value)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheSettings.AbsoluteExpirationTimeInSeconds),
            SlidingExpiration = TimeSpan.FromSeconds(_cacheSettings.SlidingExpirationTimeInSeconds)
        };

        appCache.Add(key, value, options);

        return ValueTask.CompletedTask;
    }
}
