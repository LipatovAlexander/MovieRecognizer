using Api.Infrastructure.ApiResponses;

namespace Api.Infrastructure.Authentication;

public class ApiKeyAuthenticationEndpointFilter(
    IConfiguration configuration,
    ILogger<ApiKeyAuthenticationEndpointFilter> logger) : IEndpointFilter
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<ApiKeyAuthenticationEndpointFilter> _logger = logger;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
        {
            return TypedResults.Extensions.Unauthorized(Responses.Error("missing_api_key"));
        }

        if (!extractedApiKey.Equals(_configuration[AuthConstants.ApiKeyConfigurationKey]))
        {
            _logger.LogInformation("Invalid api key '{ApiKey}'", extractedApiKey.ToString());

            return TypedResults.Extensions.Unauthorized(Responses.Error("invalid_api_key"));
        }

        return await next(context);
    }
}