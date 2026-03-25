using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace EcommerceDev.Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(10);

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            var cachedValue = await _distributedCache.GetStringAsync(key);

            if (string.IsNullOrWhiteSpace(cachedValue))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(cachedValue, _jsonSerializerOptions);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var cacheExpiration = expiration ?? _defaultExpiration;

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheExpiration
            };

            var json = JsonSerializer.Serialize(value, _jsonSerializerOptions);

            await _distributedCache.SetStringAsync(key, json, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

    }
}
