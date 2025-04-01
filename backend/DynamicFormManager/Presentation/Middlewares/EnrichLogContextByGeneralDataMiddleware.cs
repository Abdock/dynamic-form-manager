using Presentation.Constants;
using Presentation.Extensions;
using Serilog.Context;

namespace Presentation.Middlewares;

public class EnrichLogContextByGeneralDataMiddleware
{
    private readonly RequestDelegate _next;

    public EnrichLogContextByGeneralDataMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var username = httpContext.User.GetEmail();
        using var usernameProperty = LogContext.PushProperty(LoggingConstants.UsernameKey, username);
        using var correlationIdProperty = LogContext.PushProperty(LoggingConstants.CorrelationIdKey, httpContext.TraceIdentifier);
        await _next(httpContext);
    }
}