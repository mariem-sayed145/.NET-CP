using System.Text.Json;
using ECommerce.Application.Interfaces.Services;
using StackExchange.Redis;

namespace ECommerce.Infrastructure.Services;

public sealed class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var db = _redis.GetDatabase();

        var value = await db.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task SetAsync<T>(
    string key,
    T value,
    TimeSpan? expiration = null)
    {
        var db = _redis.GetDatabase();

        var json = JsonSerializer.Serialize(value);

        await db.StringSetAsync(
            key,
            json,
            expiration.HasValue
                ? new Expiration(expiration.Value)
                : Expiration.Default);
    } 
    public async Task RemoveAsync(string key)
    {
        var db = _redis.GetDatabase();

        await db.KeyDeleteAsync(key);
    }
}
