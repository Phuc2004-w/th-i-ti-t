using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class WeatherServer
{
    private HttpListener httpListener;
    private ConcurrentDictionary<string, WebSocket> clients;

    public WeatherServer()
    {
        httpListener = new HttpListener();
        clients = new ConcurrentDictionary<string, WebSocket>();
    }

    public async Task Start()
    {
        httpListener.Prefixes.Add("http://localhost:8080/");
        httpListener.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            var context = await httpListener.GetContextAsync();
            if (context.Request.IsWebSocketRequest)
            {
                var wsContext = await context.AcceptWebSocketAsync(null);
                var clientId = context.Request.Headers["Client-ID"];
                clients[clientId] = wsContext.WebSocket;
                _ = Task.Run(() => HandleClient(wsContext.WebSocket, clientId));
            }
        }
    }

    private async Task HandleClient(WebSocket socket, string clientId)
    {
        var buffer = new byte[1024];
        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var weatherData = Encoding.UTF8.GetBytes(message);
                if (clients.TryGetValue("client2", out WebSocket client2Socket))
                {
                    await client2Socket.SendAsync(new ArraySegment<byte>(weatherData), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
