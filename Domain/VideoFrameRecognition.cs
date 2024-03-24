namespace Domain;

public class VideoFrameRecognition(Guid videoFrameId, IReadOnlyCollection<RecognizedTitle> recognizedTitles)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid VideoFrameId { get; set; } = videoFrameId;
    public IReadOnlyCollection<RecognizedTitle> RecognizedTitles { get; set; } = recognizedTitles;
}

public class RecognizedTitle
{
    public required string Title { get; set; }
    public required string Subtitle { get; set; }
    public required string Description { get; set; }
    public required string Source { get; set; }
    public required Uri Link { get; set; }
    public required Uri Thumbnail { get; set; }
}