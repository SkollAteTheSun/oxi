namespace Kp.Ms.Oxi.Middlewares;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _token;

    public TokenMiddleware(RequestDelegate next, string token)
    {
        _next = next;
        _token = token;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.Headers.TryGetValue("Token", out var requestToken);
        if (string.IsNullOrEmpty(requestToken) || requestToken != _token)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Token is invalid");
        }
        else
        {
            await _next.Invoke(context);
        }
    }
}