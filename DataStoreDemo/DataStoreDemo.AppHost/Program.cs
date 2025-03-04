IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres("postgres")
    .WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(5050));
IResourceBuilder<PostgresDatabaseResource> postgresdb = postgres.AddDatabase("postgresdb");

IResourceBuilder<RedisResource> cache = builder
    .AddRedis("cache")
    .WithRedisInsight();

builder.AddProject<Projects.DataStoreDemo_API>("datastoredemo-api", configure: project =>
    {
        project.ExcludeLaunchProfile = true;
    })
    .WithReference(postgresdb)
    .WithReference(cache)
    .WaitFor(postgresdb)
    .WaitFor(cache)
    ;

builder.AddProject<Projects.DataStoreDemo_DatabaseSeeder>("datastoredemo-databaseseeder")
    .WithReference(postgresdb)
    .WithReference(cache)
    .WaitFor(postgresdb)
    .WaitFor(cache)
    ;

builder.Build().Run();
