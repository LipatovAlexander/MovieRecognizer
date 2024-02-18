namespace Domain.Entities;

public class Video(string externalId, string title, string author, TimeSpan? duration) : BaseEntity
{
    public string ExternalId { get; set; } = externalId;
    
    public string Title { get; set; } = title;

    public string Author { get; set; } = author;

    public TimeSpan? Duration { get; set; } = duration;

    public ICollection<VideoFrame> VideoFrames { get; set; } = new List<VideoFrame>();

    public static Specification<Video> WithExternalId(string externalId) =>
        new(video => video.ExternalId == externalId);
}