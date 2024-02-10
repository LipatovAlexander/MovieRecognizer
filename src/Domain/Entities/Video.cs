namespace Domain.Entities;

public class Video(string title, string author, TimeSpan duration, Uri fileUrl, Uri webSiteUrl) : BaseEntity
{
    public string Title { get; set; } = title;

    public string Author { get; set; } = author;

    public TimeSpan Duration { get; set; } = duration;

    public Uri FileUrl { get; set; } = fileUrl;

    public Uri WebSiteUrl { get; set; } = webSiteUrl;

    public ICollection<VideoFrame> VideoFrames { get; set; } = new List<VideoFrame>();
}