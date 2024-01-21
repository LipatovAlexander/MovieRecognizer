using Domain;

namespace Recognizer.Application.FramesSearch;

public sealed class FramesSearchRecognitionStrategy : IRecognitionStrategy
{
    public async Task<IList<RecognitionItem>> RecognizeAsync(Uri videoUri, CancellationToken cancellationToken)
    {
        return new List<RecognitionItem>
        {
            new()
            {
                Title = "Fake title"
            }
        };
    }
}
