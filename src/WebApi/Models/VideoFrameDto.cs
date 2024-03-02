namespace WebApi.Models;

public class VideoFrameDto
{
    public required Guid Id { get; set; }
    
    public required string ExternalId { get; set; }

    public required TimeSpan Timestamp { get; set; }
}