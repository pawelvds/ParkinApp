namespace ParkinApp.Middlewares;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<WebSocketMiddleware> _logger;
    private readonly Func<HttpContext, Task> _webSocketHandler;

    public WebSocketMiddleware(RequestDelegate next, ILogger<WebSocketMiddleware> logger, Func<HttpContext, Task> webSocketHandler)
    {
        _next = next;
        _logger = logger;
        _webSocketHandler = webSocketHandler;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest && _webSocketHandler != null)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await _webSocketHandler(context);
        }
        else
        {
            await _next(context);
        }
    }
}