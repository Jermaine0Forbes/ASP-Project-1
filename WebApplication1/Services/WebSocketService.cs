using System.Text.Json;
using WebApplication1.Data;
using WebApplication1.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Services;

public class WebSocketService(IServiceProvider serviceProvider) : BackgroundService
{
    public event Func<string, Task>? OnVisitorUpdate;
    public event Func<string, Task>? OnEmailSent;
    // public event Func<string, Task>? OnUserChanged;

    private readonly IServiceProvider _sp = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(2000, stoppingToken);

            var visitors = await GetVisitorData();

            var inventory = JsonSerializer.Serialize(new
            {
                type = "inventory",
                itemId = 456,
                stock = 10
            });

            if (OnVisitorUpdate is not null)
                await OnVisitorUpdate(visitors);

            if (OnEmailSent is not null)
                await OnEmailSent(inventory);
        }
    }


    private async Task<string> GetVisitorData(string table = "IpAddresses")
    {
        string query = $@"

            WITH Hours12 AS (
                SELECT 0 AS Hour UNION ALL
                SELECT Hour + 1 FROM Hours12 WHERE Hour < 11
            ),
            AllHours AS (
                SELECT Hour,
                    CASE Hour WHEN 0 THEN '12 AM' ELSE CAST(Hour AS VARCHAR) + ' AM' END AS Label
                FROM Hours12
                UNION ALL
                SELECT Hour + 12,
                    CASE Hour WHEN 0 THEN '12 PM' ELSE CAST(Hour AS VARCHAR) + ' PM' END AS Label
                FROM Hours12
            ),
            HourlyData AS (
                SELECT 
                    DATEPART(HOUR, CreatedAt)   AS Hour,
                    COUNT(*) AS Num
                FROM {table}
                WHERE CreatedAt  >= CAST(GETDATE() AS DATE)
                AND CreatedAt  <  DATEADD(DAY, 1, CAST(GETDATE() AS DATE))
                GROUP BY DATEPART(HOUR, CreatedAt)
            )
            SELECT 
                A.Hour AS hour,
                A.Label AS label,
                ISNULL(D.Num, 0) AS num
            FROM AllHours A
            LEFT JOIN HourlyData D ON A.Hour = D.Hour
            ORDER BY A.Hour;
            ";

        using var scope = _sp.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();

        var visitors = await context.Database.SqlQueryRaw<DailyDataViewModel>(query).ToListAsync();
        return JsonSerializer.Serialize(visitors);

    }

}