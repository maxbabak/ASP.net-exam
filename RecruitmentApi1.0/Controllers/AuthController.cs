using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RecruitmentApi1._0.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("Публічний ендпоінт");
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