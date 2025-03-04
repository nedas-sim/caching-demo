using DataStoreDemo.Database.Entities;

namespace DataStoreDemo.Database.DataProviders;

public interface IKeyValueMapProvider
{
    Task<KeyValueMap?> GetByIdAsync(string id);
    Task UpdateAsync(KeyValueMap keyValueMap);
}
