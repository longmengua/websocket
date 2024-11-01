using System.Collections.Concurrent;
using System.Net.WebSockets;

public class ChatRoomService
{
    private readonly ChatRoom _chatRoom = new();

    public void AddUser(string connectionId, string username, WebSocket webSocket)
    {
        _chatRoom.AddUser(connectionId, username);
        _chatRoom.SetWebSocket(connectionId, webSocket);
    }

    public void RemoveUser(string connectionId)
    {
        _chatRoom.RemoveUser(connectionId);
    }

    public async Task BroadcastMessage(string message, string senderId)
    {
        await _chatRoom.BroadcastMessage(message, senderId);
    }

    public IEnumerable<ChatUser> GetUsers()
    {
        return _chatRoom.GetUsers();
    }
}
