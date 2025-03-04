using DataStoreDemo.Database;
using DataStoreDemo.DatabaseSeeder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<DatabaseMaintenanceBackgroundService>();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddCaching(builder.Configuration);
builder.Services.AddDataProviders();

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
