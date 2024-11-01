using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[Route("ws")]
[ApiController]
public class WebSocketController : ControllerBase
{
    private readonly WebSocketService _webSocketService;

    public WebSocketController(WebSocketService webSocketService)
    {
        _webSocketService = webSocketService;
    }

    [HttpGet]
    public async Task AcceptWebSocket()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
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
                    var receiveMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var pushMessage = await _webSocketService.ProcessMessages(receiveMessage);
                    var buff = Encoding.UTF8.GetBytes(pushMessage);
                    await webSocket.SendAsync(new ArraySegment<byte>(buff), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
        else
        {
            HttpContext.Response.StatusCode = 400; // Bad Request
        }
    }
}
