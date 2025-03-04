using DataStoreDemo.Database.DataProviders;
using Microsoft.AspNetCore.Mvc;

namespace DataStoreDemo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DemoController : ControllerBase
{
    [HttpGet("database/{id}")]
    public async Task<IActionResult> CallDatabaseOnly(
        [FromServices] DatabaseProvider databaseProvider,
        [FromRoute] string id)
    {
        return Ok(await databaseProvider.GetByIdAsync(id));
    }

    [HttpGet("cache/{id}")]
    public async Task<IActionResult> CallCache(
        [FromServices] CacheProvider cacheProvider,
        [FromRoute] string id)
    {
        return Ok(await cacheProvider.GetByIdAsync(id));
    }

    [HttpGet("output-cache/{id}")]
    public async Task<IActionResult> FallbackFromOutputCache(
        [FromServices] CacheProvider cacheProvider,
        [FromRoute] string id)
    {
        return Ok(await cacheProvider.GetByIdAsync(id));
    }
}
