﻿using Infrastructure.WebApi.ApiResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.WebApi.Middlewares;

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