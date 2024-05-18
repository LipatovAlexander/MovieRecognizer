namespace Api.Infrastructure.Authentication;

public static class TypedResultsExtensions
{
    public static UnauthorizedHttpObjectResult<TValue> Unauthorized<TValue>(this IResultExtensions _, TValue value)
    {
        return new UnauthorizedHttpObjectResult<TValue>(value);
    }
}