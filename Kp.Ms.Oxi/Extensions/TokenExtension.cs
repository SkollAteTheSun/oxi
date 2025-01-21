using Kp.Ms.Oxi.Middlewares;

namespace Kp.Ms.Oxi.Extensions;

public static class TokenExtension
{
    public static IApplicationBuilder UseToken(this IApplicationBuilder builder, string token)
    {
        return builder.UseMiddleware<TokenMiddleware>(token);
    }
}