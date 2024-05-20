namespace Domain;

public class VideoFrameRecognition(Guid videoId, Guid videoFrameId, RecognizedTitle recognizedTitle)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid VideoId { get; } = videoId;
    public Guid VideoFrameId { get; } = videoFrameId;
    public RecognizedTitle RecognizedTitle { get; } = recognizedTitle;
}