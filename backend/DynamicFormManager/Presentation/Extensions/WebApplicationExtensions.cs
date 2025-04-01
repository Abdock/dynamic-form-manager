using Presentation.Middlewares;

namespace Presentation.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseLogContextEnrichment(this IApplicationBuilder application)
    {
        application.UseMiddleware<EnrichLogContextByGeneralDataMiddleware>();
        return application;
    }
}