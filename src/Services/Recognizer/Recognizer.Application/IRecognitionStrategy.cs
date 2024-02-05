using Domain.Entities;

namespace Recognizer.Application;

public interface IRecognitionStrategy
{
    Task<IList<RecognitionItem>> RecognizeAsync(Uri videoUri, CancellationToken cancellationToken);
}
