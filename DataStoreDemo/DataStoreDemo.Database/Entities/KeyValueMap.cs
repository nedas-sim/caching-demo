namespace DataStoreDemo.Database.Entities;

public class KeyValueMap
{
    public string Id { get; set; } = null!;
    public string Value { get; set; } = null!;

    public string InstanceCacheKey() => CacheKey(Id);
    public static string CacheKey(string id) => $"KeyValueMap:{id}";
}
