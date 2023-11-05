using WebApiExtensions.Middlewares;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class HttpExceptionHandlerConfiguration
{
    public static IApplicationBuilder UseHttpExceptionHandler(this IApplicationBuilder app) =>
        app.UseMiddleware<HttpExceptionHandler>();
}
