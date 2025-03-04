using DataStoreDemo.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataStoreDemo.Database.DataProviders;

public class DatabaseProvider(AppDbContext dbContext)
    : IKeyValueMapProvider
{
    public async Task<KeyValueMap?> GetByIdAsync(string id)
    {
        return await dbContext
            .KeyValueMaps
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(KeyValueMap keyValueMap)
    {
        await dbContext.KeyValueMaps
            .Where(x => x.Id == keyValueMap.Id)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(x => x.Value, keyValueMap.Value));
    }
}
