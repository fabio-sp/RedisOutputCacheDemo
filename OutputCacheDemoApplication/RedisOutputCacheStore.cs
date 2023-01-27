using Microsoft.AspNetCore.OutputCaching;
using StackExchange.Redis;

namespace OutputCacheDemoApplication
{
    public class RedisOutputCacheStore : IOutputCacheStore
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisOutputCacheStore(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async ValueTask<byte[]> GetAsync(string key, CancellationToken cancellationToken)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async ValueTask SetAsync(string key, byte[] value, string[] tags, TimeSpan validFor, CancellationToken cancellationToken)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.StringSetAsync(key, value, validFor);

            foreach (var tag in tags ?? Array.Empty<string>())
            {
                await db.SetAddAsync(tag, key);
            }
        }

        public async ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
        {
            var db = _connectionMultiplexer.GetDatabase();

            var taggedKeys = await db.SetMembersAsync(tag);
            var keysToDelete = taggedKeys.Select(k => (RedisKey)k.ToString());

            await db.KeyDeleteAsync(keysToDelete.ToArray());
            await db.KeyDeleteAsync(tag);
        }
    }
}
