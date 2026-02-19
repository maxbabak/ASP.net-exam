namespace RecruitmentApi1._0.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var role = context.User?.Claims
            .FirstOrDefault(c => c.Type.Contains("role"))?.Value ?? "Anonymous";

        _logger.LogInformation(
            "[{Time}] {Method} {Path} Role: {Role}",
            DateTime.UtcNow,
            context.Request.Method,
            context.Request.Path,
            role
        );

        await _next(context);
    }
}