using Microsoft.AspNetCore.Routing;

namespace WebApiExtensions.Endpoints;

public interface IEndpoint
{
    static abstract void AddRoute(IEndpointRouteBuilder builder);
}

public interface IEndpoint<TResult> : IEndpoint
{
    static abstract Task<TResult> HandleAsync(CancellationToken cancellationToken);
}

public interface IEndpoint<TResult, in TArg1> : IEndpoint
{
    static abstract Task<TResult> HandleAsync(TArg1 arg1, CancellationToken cancellationToken);
}

public interface IEndpoint<TResult, in TArg1, in TArg2> : IEndpoint
{
    static abstract Task<TResult> HandleAsync(TArg1 arg1, TArg2 arg2, CancellationToken cancellationToken);
}

public interface IEndpoint<TResult, in TArg1, in TArg2, in TArg3> : IEndpoint
{
    static abstract Task<TResult> HandleAsync(TArg1 arg1, TArg2 arg2, TArg3 arg3, CancellationToken cancellationToken);
}

public interface IEndpoint<TResult, in TArg1, in TArg2, in TArg3, in TArg4> : IEndpoint
{
    static abstract Task<TResult> HandleAsync(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, CancellationToken cancellationToken);
}

public interface IEndpoint<TResult, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5> : IEndpoint
{
    static abstract Task<TResult> HandleAsync(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, CancellationToken cancellationToken);
}

public interface IEndpoint<TResult, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6> : IEndpoint
{
    static abstract Task<TResult> HandleAsync(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, CancellationToken cancellationToken);
}

public interface IEndpoint<TResult, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7> : IEndpoint
{
    static abstract Task<TResult> HandleAsync(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, CancellationToken cancellationToken);
}

public interface IEndpoint<TResult, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8> : IEndpoint
{
    static abstract Task<TResult> HandleAsync(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, CancellationToken cancellationToken);
}
