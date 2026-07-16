using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace WebApp2.Managers;

public class WebSocketsManager
{
        // Stores active sockets mapped to a unique connection ID or Username
    private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

    public ConcurrentDictionary<string, WebSocket> GetAllSockets() => _sockets;

    public string AddSocket(WebSocket socket)
    {
        var connectionId = Guid.NewGuid().ToString();
        _sockets.TryAdd(connectionId, socket);
        return connectionId;
    }

    public async Task RemoveSocket(string id)
    {
        if (_sockets.TryRemove(id, out var socket))
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by manager", CancellationToken.None);
            }
        }
    }
}
