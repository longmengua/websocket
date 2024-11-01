using System.Collections.Concurrent;
using System.Net.WebSockets;

public class ChatRoom
{
    private readonly ConcurrentDictionary<string, WebSocket> _connections = new();
    private readonly ConcurrentDictionary<string, ChatUser> _users = new();

    public void AddUser(string connectionId, string username)
    {
        _connections.TryAdd(connectionId, null);
        _users.TryAdd(connectionId, new ChatUser { ConnectionId = connectionId, Username = username });
    }

    public void RemoveUser(string connectionId)
    {
        _connections.TryRemove(connectionId, out _);
        _users.TryRemove(connectionId, out _);
    }

    public async Task BroadcastMessage(string message, string senderId)
    {
        var sender = _users[senderId]?.Username ?? "Server";

        foreach (var connection in _connections)
        {
            if (connection.Key != senderId)
            {
                var buffer = System.Text.Encoding.UTF8.GetBytes($"{sender}: {message}");
                await connection.Value.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    public void SetWebSocket(string connectionId, WebSocket webSocket)
    {
        _connections[connectionId] = webSocket;
    }

    public IEnumerable<ChatUser> GetUsers()
    {
        return _users.Values;
    }
}
