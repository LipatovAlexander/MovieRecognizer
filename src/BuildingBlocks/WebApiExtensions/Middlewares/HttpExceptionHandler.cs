using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebApiExtensions.ApiResponses;

namespace WebApiExtensions.Middlewares;

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
            var response = Responses.Error(CommonErrorCodes.InvalidRequest, new[] { exception.Message });
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled exception occurred");
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(Responses.Error(CommonErrorCodes.InternalError));
        }
    }
}

public static class HttpExceptionHandlerConfiguration
{
    public static IApplicationBuilder UseHttpExceptionHandler(this IApplicationBuilder app) =>
        app.UseMiddleware<HttpExceptionHandler>();
}
