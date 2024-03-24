using System.Text.Json.Serialization;

namespace Api.Models;

public class VideoDto
{
    [JsonPropertyName("title")] public required string Title { get; set; }

    [JsonPropertyName("author")] public required string Author { get; set; }

    [JsonPropertyName("duration")] public required TimeSpan Duration { get; set; }

    [JsonPropertyName("video_frames")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyCollection<VideoFrameDto>? VideoFrames { get; set; }
}