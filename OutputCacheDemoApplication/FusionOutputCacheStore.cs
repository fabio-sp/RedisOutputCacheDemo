using Microsoft.AspNetCore.OutputCaching;
using ZiggyCreatures.Caching.Fusion;

namespace OutputCacheDemoApplication
{
    public class FusionOutputCacheStore : IOutputCacheStore
    {
        private readonly IFusionCache fusionCache;

        public FusionOutputCacheStore(IFusionCache fusionCache)
        {
            this.fusionCache = fusionCache;
        }

        public async ValueTask<byte[]?> GetAsync(string key, CancellationToken cancellationToken)
        {
            return await fusionCache.GetOrDefaultAsync<byte[]>(key, token: cancellationToken);
        }

        public async ValueTask SetAsync(string key, byte[] value, string[]? tags, TimeSpan validFor, CancellationToken cancellationToken)
        {
            foreach (var tag in tags ?? Array.Empty<string>())
            {
                var tagSet = await fusionCache.GetOrDefaultAsync<HashSet<string>>(tag, token: cancellationToken) ?? new HashSet<string>();
                tagSet.Add(key);
                await fusionCache.SetAsync(tag, tagSet, validFor, token: cancellationToken);
            }

            await fusionCache.SetAsync(key, value, validFor, token: cancellationToken);
        }

        public async ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
        {
            var taggedKeys = await fusionCache.GetOrDefaultAsync<HashSet<string>>(tag, token: cancellationToken) ?? new HashSet<string>();

            foreach (var key in taggedKeys)
            {
                await fusionCache.RemoveAsync(key, token: cancellationToken);
            }

            await fusionCache.RemoveAsync(tag, token: cancellationToken);
        }
    }
}