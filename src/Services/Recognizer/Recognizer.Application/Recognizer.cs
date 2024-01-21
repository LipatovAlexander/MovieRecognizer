using System.Threading.Channels;
using Domain;
using Recognizer.Application.Extensions;

namespace Recognizer.Application;

public interface IRecognitionService
{
    Task<IList<RecognitionItem>> RecognizeAsync(Uri videoUri, CancellationToken cancellationToken);
}

public sealed class RecognitionService(IEnumerable<IRecognitionStrategy> recognitionStrategies) : IRecognitionService
{
    private readonly IEnumerable<IRecognitionStrategy> _recognitionStrategies = recognitionStrategies;

    public async Task<IList<RecognitionItem>> RecognizeAsync(Uri videoUri, CancellationToken cancellationToken)
    {
        var resultChannel = CreateChannel();

        var tasks = _recognitionStrategies.Select(s =>
            ProcessStrategyAsync(s, videoUri, resultChannel, cancellationToken));

        await Task.WhenAll(tasks);

        return await resultChannel.CompleteAsync(cancellationToken);
    }

    private static async Task ProcessStrategyAsync(IRecognitionStrategy recognitionStrategy, Uri videoUri, Channel<RecognitionItem> resultChannel, CancellationToken cancellationToken)
    {
        var items = await recognitionStrategy.RecognizeAsync(videoUri, cancellationToken);

        foreach (var item in items)
        {
            await resultChannel.Writer.WriteAsync(item, cancellationToken);
        }
    }
    
    private static Channel<RecognitionItem> CreateChannel()
    {
        var channelOptions = new UnboundedChannelOptions { SingleReader = true };
        return Channel.CreateUnbounded<RecognitionItem>(channelOptions);
    }
}
