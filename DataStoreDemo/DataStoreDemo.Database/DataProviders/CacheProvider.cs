using DataStoreDemo.Database.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace DataStoreDemo.Database.DataProviders;

public class CacheProvider(IFusionCache fusionCache,
    DatabaseProvider databaseProvider)
    : IKeyValueMapProvider
{
    public async Task<KeyValueMap?> GetByIdAsync(string id)
    {
        string cacheKey = KeyValueMap.CacheKey(id);

        return await fusionCache.GetOrSetAsync(
            cacheKey, 
            (_) => databaseProvider.GetByIdAsync(id));
    }

    public async Task UpdateAsync(KeyValueMap keyValueMap)
    {
        string cacheKey = keyValueMap.InstanceCacheKey();

        await databaseProvider.UpdateAsync(keyValueMap);
        await fusionCache.SetAsync(cacheKey, keyValueMap);
    }
}
