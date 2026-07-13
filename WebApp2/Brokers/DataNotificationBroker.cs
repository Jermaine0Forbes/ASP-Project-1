namespace WebApp2.Brokers;


public class DataNotificationBroker
{
    
    // Define an event that accepts an object payload
    public event Func<object, Task>? OnDataChanged;

    // Triggered by controllers, background tasks, or database events
    public async Task PublishUpdateAsync(object payload)
    {
        if (OnDataChanged != null)
        {
            await OnDataChanged.Invoke(payload);
        }
    }


}