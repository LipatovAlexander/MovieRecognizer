using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WebApiExtensions.ApiResponses;

namespace WebApiExtensions.Middlewares;

public sealed class CustomNotFoundResponseHandler(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);
        
        if (context.Response.StatusCode != StatusCodes.Status404NotFound)
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
