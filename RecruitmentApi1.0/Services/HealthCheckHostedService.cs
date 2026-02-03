using Microsoft.EntityFrameworkCore;
using RecruitmentApi1._0.Data;
using System.Text;

namespace RecruitmentApi1._0.Services;

public class HealthCheckHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _logFilePath = "health-log.txt";

    public HealthCheckHostedService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await WriteHealthLogAsync();
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    private async Task WriteHealthLogAsync()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");

        // ===== DB CHECK =====
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await db.Database.ExecuteSqlRawAsync("SELECT 1;");
            sb.AppendLine("DB Status: OK");
        }
        catch (Exception ex)
        {
            sb.AppendLine($"DB Status: ERROR - {ex.Message}");
        }

        // ===== MEMORY CHECK =====
        long memoryBytes = GC.GetTotalMemory(false);
        long memoryMb = memoryBytes / (1024 * 1024);
        sb.AppendLine($"Memory Used: {memoryMb} MB");

        sb.AppendLine(new string('-', 40));

        await File.AppendAllTextAsync(_logFilePath, sb.ToString());
    }
}