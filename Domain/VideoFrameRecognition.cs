namespace Domain;

public class VideoFrameRecognition(Guid videoFrameId, RecognizedTitle recognizedTitle)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid VideoFrameId { get; set; } = videoFrameId;
    public RecognizedTitle RecognizedTitle { get; set; } = recognizedTitle;
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