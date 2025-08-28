using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace NorthwindApi.Infrastructure.Cache;

public class DistributedCacheService(IDistributedCache cache, ILogger<DistributedCacheService> logger) : ICacheService
{
    private readonly IDistributedCache _cache = cache;
    private readonly ILogger<DistributedCacheService> _logger = logger;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var cachedValue = await _cache.GetStringAsync(key, cancellationToken);

            return string.IsNullOrWhiteSpace(cachedValue) ? default : JsonSerializer.Deserialize<T>(cachedValue, _serializerOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving value from cache with key: {Key}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var serializedValue = JsonSerializer.Serialize(value, _serializerOptions);

            var options = new DistributedCacheEntryOptions();
            options.SetAbsoluteExpiration(expiration ?? TimeSpan.FromMinutes(30)); // Default 30 minutes

            await _cache.SetStringAsync(key, serializedValue, options, cancellationToken);
            _logger.LogDebug("Cache entry set for key: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting cache value for key: {Key}", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _cache.RemoveAsync(key, cancellationToken);
            _logger.LogDebug("Cache entry removed for key: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cache value for key: {Key}", key);
        }
    }

    // public async Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    // {
    //     _logger.LogWarning("RemoveByPatternAsync is not fully implemented for generic IDistributedCache");
    //     await Task.CompletedTask;
    // }
}