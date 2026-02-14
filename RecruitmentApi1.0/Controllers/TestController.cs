using Microsoft.AspNetCore.Mvc;
using RecruitmentApi1._0.Services;
using Serilog;

namespace RecruitmentApi1._0.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly ITestService _service;

    public TestController(ITestService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        Log.Information("GET api/test викликано");
        return Ok(_service.GetMessage());
    }
}