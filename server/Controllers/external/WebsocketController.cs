using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebSockets;
using System.Net.WebSockets;
using System.Threading.Tasks;

[Route("im-ws")]
[ApiController]
public class ImWebSocketController : ControllerBase
{
    private readonly WebSocketService _webSocketService;

    public ImWebSocketController(WebSocketService webSocketService)
    {
        _webSocketService = webSocketService;
    }

    [HttpGet]
    public async Task<IActionResult> Connect()
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            return BadRequest("WebSocket request expected.");
        }

        WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        await _webSocketService.HandleWebSocketConnection(webSocket);

        // 处理完后不需要返回结果
        return new EmptyResult();
    }
}
