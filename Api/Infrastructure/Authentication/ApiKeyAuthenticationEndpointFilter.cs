using Api.Infrastructure.ApiResponses;

namespace Api.Infrastructure.Authentication;

public class ApiKeyAuthenticationEndpointFilter(IConfiguration configuration) : IEndpointFilter
{
    private readonly IConfiguration _configuration = configuration;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
        {
            return TypedResults.Extensions.Unauthorized(Responses.Error("missing_api_key"));
        }

        if (!extractedApiKey.Equals(_configuration[AuthConstants.ApiKeyConfigurationKey]))
        {
            return TypedResults.Extensions.Unauthorized(Responses.Error("invalid_api_key"));
        }

        return await next(context);
    }
}