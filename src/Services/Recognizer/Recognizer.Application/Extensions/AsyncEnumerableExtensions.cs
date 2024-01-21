namespace Recognizer.Application.Extensions;

public static class AsyncEnumerableExtensions
{
    public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> asyncEnumerable, CancellationToken cancellationToken)
    {
        var result = new List<T>();
    
        await foreach (var item in asyncEnumerable.WithCancellation(cancellationToken))
        {
            result.Add(item);
        }

        return result;
    }
}
