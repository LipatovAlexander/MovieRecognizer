namespace Domain.Entities;

public class VideoFrame(string externalId, TimeSpan timestamp) : BaseEntity
{
    public TimeSpan Timestamp { get; set; } = timestamp;

    public string ExternalId { get; set; } = externalId;
}