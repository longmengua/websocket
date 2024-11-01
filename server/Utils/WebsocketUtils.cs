using System.Net.WebSockets;

public static class WebSocketUtils
{
    public static async Task<string> ReceiveUsernameAsync(WebSocket webSocket)
    {
        var buffer = new byte[1024];
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        return System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
    }

    public static async Task HandleWebSocketAsync(WebSocket webSocket, ChatRoomService chatRoomService, string connectionId)
    {
        var buffer = new byte[1024 * 4];
        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    break;
                }
                else
                {
                    var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await chatRoomService.BroadcastMessage(message, connectionId);
                }
            }
        }
        finally
        {
            chatRoomService.RemoveUser(connectionId);
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }
    }
}
