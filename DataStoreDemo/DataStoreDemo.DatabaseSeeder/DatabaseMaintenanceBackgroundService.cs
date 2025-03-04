using DataStoreDemo.Database;
using DataStoreDemo.Database.DataProviders;
using DataStoreDemo.Database.Entities;

namespace DataStoreDemo.DatabaseSeeder;

public class DatabaseMaintenanceBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<DatabaseMaintenanceBackgroundService> logger)
    : IHostedService
{
    private CancellationTokenSource? _cancellationTokenSource;
    private Task? _updateTask;
    private List<KeyValueMap>? _entries;

    public async Task StartAsync(CancellationToken _)
    {
        using (IServiceScope serviceScope = scopeFactory.CreateScope())
        {
            AppDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            logger.LogInformation("Starting database maintenance background service.");

            await dbContext.Database.EnsureCreatedAsync(CancellationToken.None);

            logger.LogInformation("Database created.");

            _entries = Enumerable
                .Range(0, 3000)
                .Select(index => new KeyValueMap
                {
                    Id = $"Id_{index}",
                    Value = Guid.NewGuid().ToString()
                })
                .ToList();

            dbContext.KeyValueMaps.AddRange(_entries);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            logger.LogInformation("Added entries to the database");

            _cancellationTokenSource = new();
        }
        
        _updateTask = 
            Task.Run(async () =>
            {
                while (true)
                {
                    using IServiceScope serviceScope = scopeFactory.CreateScope();
                    CacheProvider cacheProvider = serviceScope.ServiceProvider.GetRequiredService<CacheProvider>();

                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        break;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(10));

                    logger.LogInformation("Started updating entries in persistence");

                    foreach (KeyValueMap entry in _entries)
                    {
                        KeyValueMap newEntry = new()
                        {
                            Id = entry.Id,
                            Value = Guid.NewGuid().ToString(),
                        };

                        await cacheProvider.UpdateAsync(newEntry);
                    }

                    logger.LogInformation("Finished updating entries in persistence");
                }
            }, CancellationToken.None);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping database service");

        _cancellationTokenSource?.Cancel();

        if (_updateTask != null)
        {
            await _updateTask;
        }
    }
}
