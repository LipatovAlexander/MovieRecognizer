using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using WebApiExtensions.MinimalApi;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class EndpointsConfiguration
{
    public static IServiceCollection AddEndpoints<TAssemblyMarker>(this IServiceCollection services)
    {
        return services.AddEndpoints(typeof(TAssemblyMarker).Assembly);
    }

    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly sourceAssembly)
    {
        var endpoints = sourceAssembly.GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IEndpoint)))
            .Where(t => !t.IsInterface);

        foreach (var endpoint in endpoints)
        {
            services.AddScoped(typeof(IEndpoint), endpoint);
        }

        return services;
    }

    public static RouteGroupBuilder MapEndpoints(this WebApplication builder)
    {
        var scope = builder.Services.CreateScope();

        var group = builder.MapGroup("api");

        var endpoints = scope.ServiceProvider.GetServices<IEndpoint>();

        foreach (var endpoint in endpoints)
        {
            endpoint.AddRoute(group);
        }

        return group;
    }
}