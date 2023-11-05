using Microsoft.AspNetCore.Http;
using WebApiExtensions.ApiResponses;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public sealed class CustomNotFoundResponseHandler(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);
        
        if (context.Response.StatusCode != StatusCodes.Status404NotFound || context.Response.HasStarted)
        {
            return;
        }

        await context.Response.WriteAsJsonAsync(Responses.Error(CommonErrorCodes.NotFound));
    }
}

public static class CustomNotFoundResponseHandlerConfiguration
{
    public static IApplicationBuilder UseCustomNotFoundResponseHandler(this IApplicationBuilder app) =>
        app.UseMiddleware<CustomNotFoundResponseHandler>();
}
