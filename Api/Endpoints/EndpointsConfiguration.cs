using Api.Endpoints;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Routing;

public static class EndpointsConfiguration
{
    public static void MapEndpoint<TEndpoint>(this IEndpointRouteBuilder builder) where TEndpoint : IEndpoint
    {
        TEndpoint.AddRoute(builder);
    }
}
