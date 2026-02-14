using Microsoft.EntityFrameworkCore;
using RecruitmentApi1._0.Data;
using Serilog;

namespace RecruitmentApi1._0.Services;

public class HealthCheckHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public HealthCheckHostedService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information("HealthCheckHostedService стартував");

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckAsync();
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    private async Task CheckAsync()
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await db.Database.ExecuteSqlRawAsync("SELECT 1;");

            var memoryMb = GC.GetTotalMemory(false) / (1024 * 1024);

            Log.Information("Health OK | Memory: {Memory} MB", memoryMb);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Health check FAILED");
        }
    }
}