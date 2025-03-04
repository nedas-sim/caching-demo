using DataStoreDemo.API.Controllers;
using DataStoreDemo.Database.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace DataStoreDemo.API.Middlewares;

public class OutputCacheMiddleware(IFusionCache fusionCache) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.RouteValues["action"] is not nameof(DemoController.FallbackFromOutputCache))
        {
            await next(context);
            return;
        }

        string id = (context.Request.RouteValues["id"] as string)!;
        string cacheKey = KeyValueMap.CacheKey(id);
        MaybeValue<KeyValueMap> maybeValue = await fusionCache.TryGetAsync<KeyValueMap>(cacheKey);

        if (!maybeValue.HasValue)
        {
            await next(context);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsJsonAsync(maybeValue.Value);
    }
}
