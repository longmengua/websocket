using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class WebSocketService
{
    public async Task HandleWebSocketConnection(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];

        if (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }
            else
            {
                // Process the received message (you can add your own logic here)
                var receiveMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var pushMessage = await this.ProcessMessages(receiveMessage);
                var buff = Encoding.UTF8.GetBytes(pushMessage);
                await webSocket.SendAsync(new ArraySegment<byte>(buff), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    public async Task<string> ProcessMessages(string message)
    {
        Console.WriteLine($"Received: {message}");
        return "hello world";
    }
}
