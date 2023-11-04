using Microsoft.AspNetCore.Routing;

namespace WebApiExtensions.MinimalApi;

public interface IEndpoint
{
    void AddRoute(IEndpointRouteBuilder app);
}

public interface IEndpoint<TResult> : IEndpoint
{
    Task<TResult> HandleAsync(CancellationToken cancellationToken);
}

public interface IEndpoint<in TRequest, TResult> : IEndpoint
{
    Task<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
