using WebApiExtensions.Middlewares;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class CustomNotFoundResponseHandlerConfiguration
{
    public static IApplicationBuilder UseCustomNotFoundResponseHandler(this IApplicationBuilder app) =>
        app.UseMiddleware<CustomNotFoundResponseHandler>();
}
