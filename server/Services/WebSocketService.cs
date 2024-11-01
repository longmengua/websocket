using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
public class WebSocketService
{
    public async Task<string> ProcessMessages(string message)
    {
        Console.WriteLine($"Received: {message}");
        return "hello world";
    }
}
