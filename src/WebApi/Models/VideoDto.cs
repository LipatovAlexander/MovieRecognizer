namespace WebApi.Models;

public class VideoDto
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }

    public required string Author { get; set; }

    public required TimeSpan Duration { get; set; }

    public required Uri FileUrl { get; set; }

    public required Uri WebSiteUrl { get; set; }

    public required ICollection<VideoFrameDto> VideoFrames { get; set; }
}