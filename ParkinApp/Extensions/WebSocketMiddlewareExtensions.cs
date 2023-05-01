using Microsoft.AspNetCore.WebSockets;
using WebSocketMiddleware = ParkinApp.Middlewares.WebSocketMiddleware;

namespace ParkinApp.Extensions;

public static class WebSocketMiddlewareExtensions
{
    public static IApplicationBuilder UseWebSocketMiddleware(this IApplicationBuilder app, Func<HttpContext, Task> webSocketHandler)
    {
        return app.UseMiddleware<WebSocketMiddleware>(webSocketHandler);
    }
    
}