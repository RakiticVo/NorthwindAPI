using Microsoft.Extensions.Logging;
using NorthwindApi.Infrastructure.Cache;

namespace NorthwindApi.Application.Cache;

public class BusinessCacheService(ICacheService cache, ILogger<BusinessCacheService> logger)
{
    public async Task<T?> GetOrSetAsync<T>(
        string key,
        Func<Task<T>> getItem,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        var cachedValue = await cache.GetAsync<T>(key, cancellationToken);
        if (cachedValue != null)
        {
            logger.LogDebug("Cache hit for key: {Key}", key);
            return cachedValue;
        }

        logger.LogDebug("Cache miss for key: {Key}", key);
        var item = await getItem();
        if (item != null)
        {
            await cache.SetAsync(key, item, expiration, cancellationToken);
        }

        return item;
    }
}