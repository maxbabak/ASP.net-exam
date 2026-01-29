using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace RecruitmentApi1._0.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly IMemoryCache _cache;

    public TestController(IMemoryCache cache)
    {
        _cache = cache;
    }

    [HttpGet("cached-time")]
    public IActionResult GetCachedTime()
    {
        const string cacheKey = "current_time";

        if (!_cache.TryGetValue(cacheKey, out string time))
        {
            time = DateTime.Now.ToString("HH:mm:ss");

            _cache.Set(
                cacheKey,
                time,
                TimeSpan.FromSeconds(30) // кеш на 30 секунд
            );
        }

        return Ok(new
        {
            Time = time,
            Cached = true
        });
    }

    [Authorize]
    [HttpGet("authorized")]
    public IActionResult Authorized()
    {
        return Ok("Доступ тільки з токеном");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult AdminOnly()
    {
        return Ok("Доступ тільки для Admin");
    }
}