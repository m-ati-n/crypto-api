using Microsoft.Extensions.Caching.Memory;

namespace CryptoMarket.Caching;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public T? Get<T>(string key)
    {
        _memoryCache.TryGetValue(key, out T? value);
        return value;
    }

    public void Set<T>(string key, T value, TimeSpan expiration)
    {
        CacheKeys.Keys.Add(key);

        _memoryCache.Set(key, value, expiration);
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }

    public void RemoveByPrefix(string prefix)
    {
        var keys = CacheKeys.Keys
            .Where(k => k.StartsWith(prefix))
            .ToList();

        foreach (var key in keys)
        {
            _memoryCache.Remove(key);
            CacheKeys.Keys.Remove(key);
        }
    }
}