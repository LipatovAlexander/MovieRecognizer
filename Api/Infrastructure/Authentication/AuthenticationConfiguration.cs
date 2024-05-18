using Api.Infrastructure.Authentication;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class AuthenticationConfiguration
{
    public static IEndpointConventionBuilder RequireApiKeyAuthentication(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}