namespace Domain;

public class VideoFrameRecognition(Guid videoFrameId, RecognizedTitle recognizedTitle)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid VideoFrameId { get; set; } = videoFrameId;
    public RecognizedTitle RecognizedTitle { get; set; } = recognizedTitle;
}