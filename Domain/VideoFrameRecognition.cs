namespace Domain;

public class VideoFrameRecognition(Guid videoId, Guid videoFrameId, RecognizedTitle recognizedTitle)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid VideoId { get; set; } = videoId;
    public Guid VideoFrameId { get; set; } = videoFrameId;
    public RecognizedTitle RecognizedTitle { get; set; } = recognizedTitle;
}