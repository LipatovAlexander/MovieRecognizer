using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebApiExtensions.ApiResponses;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public sealed class HttpExceptionHandler(RequestDelegate next, ILogger<HttpExceptionHandler> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BadHttpRequestException exception)
        {
            context.Response.StatusCode = exception.StatusCode;
            var response = Responses.Error(CommonErrorCodes.InvalidRequest, exception.Message);
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled exception occurred");
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = Responses.Error(CommonErrorCodes.InternalError);
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

public static class HttpExceptionHandlerConfiguration
{
    public static IApplicationBuilder UseHttpExceptionHandler(this IApplicationBuilder app) =>
        app.UseMiddleware<HttpExceptionHandler>();
}
