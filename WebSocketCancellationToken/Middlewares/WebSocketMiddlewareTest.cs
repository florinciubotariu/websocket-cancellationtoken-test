using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebSocketCancellationToken.Middlewares
{
  public class WebSocketMiddlewareTest
  {
    public WebSocketMiddlewareTest(RequestDelegate next) { }

    public async Task Invoke(HttpContext context)
    {
      if (!context.WebSockets.IsWebSocketRequest) return;

      var webSocket = await context.WebSockets.AcceptWebSocketAsync();

      var cancellationToken = new CancellationTokenSource(10000);

      try
      {
        while (true)
        {
          WebSocketReceiveResult result;
          var buffer = new byte[1024 * 4];
          do
          {
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken.Token);
          } while (!result.EndOfMessage);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }
  }
}
