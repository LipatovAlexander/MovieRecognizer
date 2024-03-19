namespace Domain;

public class VideoFrame(Guid videoId, TimeSpan timestamp, string externalId)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid VideoId { get; set; } = videoId;
    public TimeSpan Timestamp { get; set; } = timestamp;
    public string ExternalId { get; set; } = externalId;
}