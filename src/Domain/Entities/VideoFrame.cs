namespace Domain.Entities;

public class VideoFrame(Uri storageUrl, TimeSpan timestamp) : BaseEntity
{
    public required Video Video { get; set; }

    public Uri StorageUrl { get; set; } = storageUrl;

    public TimeSpan Timestamp { get; set; } = timestamp;
}