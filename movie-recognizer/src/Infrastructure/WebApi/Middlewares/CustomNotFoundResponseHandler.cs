﻿using Infrastructure.WebApi.ApiResponses;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.WebApi.Middlewares;

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