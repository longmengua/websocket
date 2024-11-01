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
            await _webSocketService.HandleWebSocketConnection(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = 400; // Bad Request
        }
    }
}
