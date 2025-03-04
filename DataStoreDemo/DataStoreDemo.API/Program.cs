using DataStoreDemo.API.Middlewares;
using DataStoreDemo.Database;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddCaching(builder.Configuration);
builder.Services.AddDataProviders();
builder.Services.AddScoped<OutputCacheMiddleware>();

WebApplication app = builder.Build();

app.UseMiddleware<OutputCacheMiddleware>();

app.MapDefaultEndpoints();

app.MapOpenApi();
app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/openapi/v1.json", "Docs"));

app.UseAuthorization();

app.MapControllers();

app.Run();
