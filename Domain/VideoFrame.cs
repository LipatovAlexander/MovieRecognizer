namespace Domain;

public class VideoFrame(Guid videoId, TimeSpan timestamp, string externalId)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid VideoId { get; } = videoId;
    public TimeSpan Timestamp { get; } = timestamp;
    public string ExternalId { get; } = externalId;
    public bool Processed { get; set; }
}