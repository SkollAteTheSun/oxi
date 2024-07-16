using Kp.Ms.Oxi.Attributes;
using Microsoft.AspNetCore.Http.Features;

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
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        var attribute = endpoint?.Metadata.GetMetadata<AuthTokenAttribute>();
        if (attribute == null)
        {
            await _next.Invoke(context);
        }
        else
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
}