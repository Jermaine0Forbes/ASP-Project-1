using System.Text.Json;

namespace WebApplication1.Services;
public class WebSocketService : BackgroundService
{
    public event Func<string, Task>? OnVisitorUpdate;
    public event Func<string, Task>? OnEmailSent;
    // public event Func<string, Task>? OnUserChanged;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(2000, stoppingToken);

            var order = JsonSerializer.Serialize(new
            {
                type = "order",
                orderId = 123,
                status = "shipped"
            });

            var inventory = JsonSerializer.Serialize(new
            {
                type = "inventory",
                itemId = 456,
                stock = 10
            });

            if (OnVisitorUpdate is not null)
                await OnVisitorUpdate(order);

            if (OnEmailSent is not null)
                await OnEmailSent(inventory);
        }
    }

}