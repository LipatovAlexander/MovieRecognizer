using System.Threading.Channels;

namespace Recognizer.Application.Extensions;

public static class ChannelExtensions
{
    public static async Task<List<T>> CompleteAsync<T>(this Channel<T> channel, CancellationToken cancellationToken)
    {
        channel.Writer.Complete();

        return await channel.Reader
            .ReadAllAsync(cancellationToken)
            .ToListAsync(cancellationToken);
    }
}