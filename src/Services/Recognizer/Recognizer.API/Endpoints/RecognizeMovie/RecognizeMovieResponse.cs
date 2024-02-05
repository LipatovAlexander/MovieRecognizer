using Domain.Entities;

namespace Recognizer.API.Endpoints.RecognizeMovie;

public sealed class RecognizeMovieResponse
{
    public required IEnumerable<RecognitionItem> Items { get; set; }
}
