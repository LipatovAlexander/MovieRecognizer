using System.Reflection;
using Microsoft.AspNetCore.Http.Metadata;

namespace Api.Infrastructure.Authentication;

public class UnauthorizedHttpObjectResult<TValue>(TValue value) :
    IResult,
    IEndpointMetadataProvider,
    IStatusCodeHttpResult,
    IValueHttpResult,
    IValueHttpResult<TValue>
{
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = StatusCode;
        await httpContext.Response.WriteAsJsonAsync(Value);
    }

    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        builder.Metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status401Unauthorized, typeof(TValue), [
            "application/json"
        ]));
    }

    public int StatusCode => StatusCodes.Status401Unauthorized;

    int? IStatusCodeHttpResult.StatusCode => StatusCode;

    object? IValueHttpResult.Value => Value;

    public TValue? Value { get; } = value;
}