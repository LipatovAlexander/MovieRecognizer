namespace Domain.Entities;

public class VideoFrame(TimeSpan timestamp) : BaseEntity
{
    public TimeSpan Timestamp { get; set; } = timestamp;

    public required File File { get; set; }
}